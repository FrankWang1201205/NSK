using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;
using System.Threading;

namespace SMART.EBMS.Controllers
{
    public partial class MatImgController : Controller
    {
        IUserService IU = new UserService();
        IMatImageService IMatIMG = new MatImageService();
        IMaterialService IMat = new MaterialService();
        IBrandService IB = new BrandService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //产品图片
    public partial class MatImgController : Controller
    {
        public ActionResult Imglibrary()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();

            PageList<MatImage> PList = IMatIMG.Get_MatImage_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Imglibrary_Sub()
        {
            MatImage IMG = new MatImage();

            List<string> BList = IB.Get_Brand_Name_List(MyUser().LinkMainCID);
            ViewData["BList"] = BList;
            return PartialView(IMG);
        }

        [HttpPost]
        public RedirectToRouteResult Imglibrary_Sub_Post(FormCollection FC)
        {
            try
            {
                HttpPostedFileBase ImgPath = Request.Files["ImagePath"];
                MyAlbumImageUpload MyUpload = new MyAlbumImageUpload();
                string[] ImgArray = MyUpload.CreateMatImgProcess(ImgPath, "/Material");

                MatImage IMG = new MatImage();
                IMG.MatSourceImgPath = ImgArray[0];
                IMG.MatImgPath = ImgArray[1];
                IMG.MatThumbImgPath = ImgArray[2];

                TryUpdateModel<MatImage>(IMG, FC);
                IMatIMG.Create_MatImage(IMG, this.MyUser().UID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Imglibrary");
        }


        public PartialViewResult Imglibrary_Sub_Update(Guid ID)
        {
            MatImage IMG = IMatIMG.Get_MatImage(ID);
            return PartialView(IMG);
        }

        [HttpPost]
        public RedirectToRouteResult Imglibrary_Sub_Update_Post(Guid ID)
        {
            try
            {
                Guid IMGID = ID;
                HttpPostedFileBase ImgPath = Request.Files["ImagePath"];
                MyAlbumImageUpload MyUpload = new MyAlbumImageUpload();
                string[] ImgArray = MyUpload.CreateMatImgProcess(ImgPath, "/Material");

                MatImage IMG = new MatImage();
                IMG.MatSourceImgPath = ImgArray[0];
                IMG.MatImgPath = ImgArray[1];
                IMG.MatThumbImgPath = ImgArray[2];

                IMatIMG.Upload_MatImage(IMGID, IMG);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Imglibrary");
        }

        [HttpPost]
        public string Imglibrary_Sub_Update_Clean(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid IMGID = ID;
                IMatIMG.Clean_MatImage_ImgPath(IMGID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }

        public string Imglibrary_Sub_Delete(Guid ID)
        {
            string result = string.Empty;
            try
            {
                Guid IMGID = ID;
                IMatIMG.Delete_MatImage_Item(IMGID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }

    //产品关联
    public partial class MatImgController : Controller
    {
        public ActionResult ImgLinkMat()
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? string.Empty : Request["MatBrand"].Trim();

            PageList<MatImage> PList = IMatIMG.Get_MatImage_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult ImgLinkMat_Sub(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid IMGID = ID;
            MatImage IMG = IMatIMG.Get_MatImage(ID);
            ViewData["IMG"] = IMG;

            Material_Filter MF = new Material_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"]); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Link_IMGID = Guid.Empty;
            try { MF.Link_IMGID = new Guid(Request["Link_IMGID"].ToString()); } catch { MF.Link_IMGID = Guid.Empty; }

            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            MF.MatBrand = Request["MatBrand"] == null ? IMG.MatBrand : Request["MatBrand"].Trim();
            PageList<Material> PList = IMat.Get_Material_PageList_For_MatImg(MF);
            ViewData["MF"] = MF;

            return View(PList);
        }

        [HttpPost]
        public string ImgLinkMat_Sub_Post(Guid ID, FormCollection FC)
        {
            string result = string.Empty;
            Guid Link_IMDID = Guid.Empty;
            Guid Link_IMGID = Guid.Empty;
            try
            {
                try { Link_IMGID = new Guid(Request["Link_IMGID"].ToString()); } catch { }
                List<Guid> MatIDList = CommonLib.GuidListStrToGuidArray(Request["MatID"]);
                IMatIMG.Set_Material_IMGID_And_IMDID_Batch(MatIDList, Link_IMGID, Link_IMDID);
            }
            catch (Exception Ex)
            {
                result = Ex.Message.ToString();
            }
            return result;
        }
    }


}