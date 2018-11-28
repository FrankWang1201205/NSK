using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMART.Api;
using SMART.Api.Models;

namespace SMART.EBMS.Controllers
{
    [Authorize]
    public partial class BrandController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        IBrandService IB = new BrandService();
        ISupplierService ISup = new SupplierService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    //品牌资料
    public partial class BrandController : Controller
    {
        public ActionResult Brand_Update()
        {
            User U = this.MyUser();
            ViewData["User"] = U;
            Brand_Filter MF = new Brand_Filter();
            try { MF.PageIndex = Convert.ToInt32(Request["PageIndex"].ToString()); } catch { }
            MF.PageIndex = MF.PageIndex <= 0 ? 1 : MF.PageIndex;
            MF.PageSize = 99;
            MF.LinkMainCID = U.LinkMainCID;
            MF.Keyword = Request["Keyword"] == null ? string.Empty : Request["Keyword"].Trim();
            PageList<SMART.Api.Models.Brand> PList = IB.Get_Brand_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Brand_Update_Add()
        {
            return PartialView();
        }

        public RedirectToRouteResult Brand_Add_Post(FormCollection FC)
        {
            User U = this.MyUser();
            try
            {
                Brand B = new Brand();
                TryUpdateModel<Brand>(B, FC);

                try
                {
                    HttpPostedFileBase BrandLogo = Request.Files["ImagePath"];
                    MyAlbumImageCropUpload MyIMG = new MyAlbumImageCropUpload();
                    B.Logo = MyIMG.CreateImgProcess(BrandLogo, 250, 250, "Brand/" + U.LinkMainCID.ToString());
                }
                catch { }

                Guid BID = IB.Create_Brand(B, U.UID);
                TempData["Success_NewBrand"] = "品牌创建成功";
                return RedirectToAction("Brand_Update");
            }
            catch (Exception Ex)
            {
                TempData["Error_NewBrand"] = Ex.Message.ToString();
                return RedirectToAction("Brand_Update");
            }
        }

        public PartialViewResult Brand_Update_Sub(Guid ID)
        {
            Guid BID = ID;
            Brand B = IB.Get_Brand_Item(BID);
            return PartialView(B);
        }

        public RedirectToRouteResult Brand_Update_Sub_Post(Guid ID, FormCollection FC)
        {
            Guid BID = ID;
            User U = this.MyUser();
            try
            {
                Brand B = new Brand();
                TryUpdateModel<Brand>(B, FC);
                IB.Set_Brand_Item(BID, B);
                TempData["Success"] = "品牌更新成功";
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Brand_Update");
        }

        public RedirectToRouteResult Brand_Update_Sub_Delete(Guid ID)
        {
            Guid BID = ID;
            try
            {
                IB.Delete_Brand(BID);
            }
            catch (Exception Ex)
            {
                TempData["Error"] = Ex.Message.ToString();
            }
            return RedirectToAction("Brand_Update");
        }

        public ActionResult Brand_Update_Sub_Img(Guid ID)
        {
            User U = this.MyUser();
            ViewData["User"] = U;

            Guid BID = ID;
            Brand B = IB.Get_Brand_Item(BID);
            return View(B);
        }

        public RedirectToRouteResult Brand_Update_Sub_Logo_Post(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;
            try
            {
                HttpPostedFileBase BrandLogo = Request.Files["ImagePath"];
                if (BrandLogo != null && BrandLogo.ContentLength > 1)
                {
                    MyAlbumImageCropUpload MyIMG = new MyAlbumImageCropUpload();
                    string BrandLogo_IMG = MyIMG.CreateImgProcess(BrandLogo, 250, 250, "Brand/" + U.LinkMainCID.ToString());
                    IB.Upload_Logo(BID, BrandLogo_IMG);
                }
            }
            catch (Exception Ex)
            {
                TempData["Error_Logo"] = Ex.Message.ToString();
            }
            return RedirectToAction("Brand_Update_Sub_Img", new { ID = ID });
        }

        public RedirectToRouteResult Brand_Update_Sub_Logo_Delete(Guid ID)
        {
            Guid BID = ID;
            IB.Delete_Logo(BID);
            return RedirectToAction("Brand_Update_Sub_Img", new { ID = ID });
        }

        public RedirectToRouteResult Brand_Update_Sub_Certificate_Post(Guid ID)
        {
            User U = this.MyUser();
            Guid BID = ID;
            try
            {
                HttpPostedFileBase File = Request.Files["FilePath"];
                if (File != null && File.ContentLength > 1)
                {
                    MyNormalUploadFile MyUpload = new MyNormalUploadFile();
                    string FilePath = MyUpload.NormalUpLoadFileProcess(File, "Brand/" + U.LinkMainCID.ToString());
                    IB.Upload_Certificate(BID, FilePath);
                }
            }
            catch (Exception Ex)
            {
                TempData["Error_Certificate"] = Ex.Message.ToString();
            }
            return RedirectToAction("Brand_Update_Sub_Img", new { ID = ID });
        }

        public RedirectToRouteResult Brand_Update_Sub_Certificate_Delete(Guid ID)
        {
            Guid BID = ID;
            IB.Delete_Certificate(BID);
            return RedirectToAction("Brand_Update_Sub_Img", new { ID = ID });
        }


    }
}