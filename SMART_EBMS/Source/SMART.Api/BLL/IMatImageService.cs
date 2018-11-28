using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMART.Api.Models;
using System.Data.Entity;

namespace SMART.Api
{
    public interface IMatImageService
    {
        PageList<MatImage> Get_MatImage_PageList(Material_Filter MF);
        MatImage Get_MatImage(Guid IMGID);
        MatImage_Detail Get_MatImage_Detail(Guid IMDID);

        Guid Create_MatImage(MatImage MatIMG, Guid UID);
        void Upload_MatImage(Guid IMGID, MatImage MatIMG);
        void Clean_MatImage_ImgPath(Guid IMGID);
        void Delete_MatImage_Item(Guid IMGID);

        void Create_MatImage_Detail(Guid Link_IMGID, string Detail_Html_Str);
        void Set_MatImage_Detail(Guid IMDID, string Detail_Html_Str);
        void Delete_MatImage_Detail(Guid IMDID);

        void Set_Material_IMGID_And_IMDID_Batch(List<Guid> MatIDList, Guid IMGID, Guid IMDID);
    }

    public partial class MatImageService : IMatImageService
    {
        SmartdbContext db = new SmartdbContext();
    }

    public partial class MatImageService : IMatImageService
    {
        /// <summary>
        /// Keyword,MatBrand
        /// </summary>
        public PageList<MatImage> Get_MatImage_PageList(Material_Filter MF)
        {
            var query = db.MatImage.Where(x => x.LinkMainCID == MF.LinkMainCID).AsQueryable();
            MF.BrandList = query.Where(x => x.MatBrand != string.Empty).Select(x => x.MatBrand).Distinct().OrderBy(x => x).ToList();

            if (!string.IsNullOrEmpty(MF.Keyword))
            {
                query = query.Where(x => x.MatName.Contains(MF.Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.MatBrand))
            {
                query = query.Where(x => x.MatBrand == MF.MatBrand).AsQueryable();
            }

            List<MatImage> RowsList = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<Guid> IMGID_List = RowsList.Select(x => x.IMGID).ToList();
            List<Guid> MatIDList = db.Material.Where(x => IMGID_List.Contains(x.Link_IMGID)).Select(x => x.Link_IMGID).ToList();

            foreach(var x in RowsList)
            {
                x.MatCount = MatIDList.Where(c => c == x.IMGID).Count();
            }

            PageList<MatImage> PList = new PageList<MatImage>();
            PList.PageIndex = MF.PageIndex;
            PList.PageSize = MF.PageSize;
            PList.TotalRecord = query.Count();
            PList.Rows = RowsList;
            return PList;
        }

        public MatImage Get_MatImage(Guid IMGID)
        {
            MatImage MatIMG = db.MatImage.Find(IMGID);
            MatIMG = MatIMG == null ? new MatImage() : MatIMG;
            MatIMG.Detail_List = db.MatImage_Detail.Where(x => x.Link_IMGID == MatIMG.IMGID).OrderBy(x => x.Create_DT).ToList();
            return MatIMG;
        }

        public MatImage_Detail Get_MatImage_Detail(Guid IMDID)
        {
            MatImage_Detail Detail = db.MatImage_Detail.Find(IMDID);
            Detail = Detail == null ? new MatImage_Detail() : Detail;
            return Detail;
        }

        public Guid Create_MatImage(MatImage MatIMG, Guid UID)
        {
            User U = db.User.Find(UID);
            if (U == null)
            {
                throw new Exception("User Is Null");
            }

            MatIMG.IMGID = MyGUID.NewGUID();
            MatIMG.MatName = MatIMG.MatName == null ? string.Empty : MatIMG.MatName.Trim();
            MatIMG.MatBrand = MatIMG.MatBrand == null ? string.Empty : MatIMG.MatBrand.Trim();
            MatIMG.MatThumbImgPath = MatIMG.MatThumbImgPath == null ? string.Empty : MatIMG.MatThumbImgPath.Trim();
            MatIMG.MatImgPath = MatIMG.MatImgPath == null ? string.Empty : MatIMG.MatImgPath.Trim();
            MatIMG.MatSourceImgPath = MatIMG.MatSourceImgPath == null ? string.Empty : MatIMG.MatSourceImgPath.Trim();
            MatIMG.Create_Person = U.UserFullName;
            MatIMG.LinkMainCID = U.LinkMainCID;
            db.MatImage.Add(MatIMG);

            if(string.IsNullOrEmpty(MatIMG.MatImgPath))
            {
                throw new Exception("未上传图片");
            }

            MyDbSave.SaveChange(db);
            return MatIMG.IMGID;
        }

        public void Upload_MatImage(Guid IMGID, MatImage MatIMG)
        {
            MatImage OLD_MatImg = db.MatImage.Find(IMGID);
            OLD_MatImg.MatThumbImgPath = MatIMG.MatThumbImgPath == null ? string.Empty : MatIMG.MatThumbImgPath.Trim();
            OLD_MatImg.MatImgPath = MatIMG.MatImgPath == null ? string.Empty : MatIMG.MatImgPath.Trim();
            OLD_MatImg.MatSourceImgPath = MatIMG.MatSourceImgPath == null ? string.Empty : MatIMG.MatSourceImgPath.Trim();
            db.Entry(OLD_MatImg).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Clean_MatImage_ImgPath(Guid IMGID)
        {
            MatImage OLD_MatImg = db.MatImage.Find(IMGID);
            OLD_MatImg.MatThumbImgPath = string.Empty;
            OLD_MatImg.MatImgPath = string.Empty;
            OLD_MatImg.MatSourceImgPath = string.Empty;
            db.Entry(OLD_MatImg).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_MatImage_Item(Guid IMGID)
        {
            MatImage OLD_MatImg = db.MatImage.Find(IMGID);
            db.MatImage.Remove(OLD_MatImg);

            List<MatImage_Detail> DetailList = db.MatImage_Detail.Where(x => x.Link_IMGID == OLD_MatImg.IMGID).ToList();
            db.MatImage_Detail.RemoveRange(DetailList);

            //清理图片关联及详情关联属性
            List<Material> MatList = new List<Material>();
            foreach (var x in MatList)
            {
                x.Link_IMGID = Guid.Empty;
                x.Link_IMDID = Guid.Empty;
            }
            MyDbSave.SaveChange(db);
        }

        public void Create_MatImage_Detail(Guid Link_IMGID, string Detail_Html_Str)
        {
            MatImage MatImg = db.MatImage.Find(Link_IMGID);
            MatImage_Detail Detail = new MatImage_Detail();
            Detail.IMDID = MyGUID.NewGUID();
            Detail.Create_DT = DateTime.Now;
            Detail.Detail_Html_Str = Detail_Html_Str == null ? string.Empty : Detail_Html_Str.Trim();
            Detail.Link_IMGID = MatImg.IMGID;
            Detail.LinkMainCID = MatImg.LinkMainCID;
            db.MatImage_Detail.Add(Detail);
            MyDbSave.SaveChange(db);
        }

        public void Set_MatImage_Detail(Guid IMDID, string Detail_Html_Str)
        {
            MatImage_Detail OLD_Detail = db.MatImage_Detail.Find(IMDID);
            OLD_Detail.Detail_Html_Str = Detail_Html_Str == null ? string.Empty : Detail_Html_Str.Trim();
            db.Entry(OLD_Detail).State = EntityState.Modified;
            MyDbSave.SaveChange(db);
        }

        public void Delete_MatImage_Detail(Guid IMDID)
        {
            MatImage_Detail OLD_Detail = db.MatImage_Detail.Find(IMDID);
            db.MatImage_Detail.Remove(OLD_Detail);

            //清理详情关联属性
            List<Material> MatList = db.Material.Where(x => x.Link_IMDID == IMDID).ToList();
            foreach (var x in MatList)
            {
                x.Link_IMDID = Guid.Empty;
            }

            MyDbSave.SaveChange(db);
        }

        public void Set_Material_IMGID_And_IMDID_Batch(List<Guid> MatIDList, Guid IMGID, Guid IMDID)
        {
            List<Material> MatList = db.Material.Where(x => MatIDList.Contains(x.MatID)).ToList();
            foreach (var x in MatList)
            {
                x.Link_IMGID = IMGID;
            }
            MyDbSave.SaveChange(db);

        }

    }

}
