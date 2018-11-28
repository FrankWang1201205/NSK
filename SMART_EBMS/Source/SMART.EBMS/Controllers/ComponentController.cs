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
    public partial class ComponentController : Controller
    {
        IUserService IU = new UserService();
        IMaterialService IMat = new MaterialService();
        private User MyUser() { return IU.Get_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class ComponentController : Controller
    {
        public PartialViewResult Mat_Proview_Part(Guid ID)
        {
            Guid MatID = ID;
            Material M = IMat.Get_Material_Item(MatID);
            return PartialView(M);
        }

        public PartialViewResult Mat_Proview_Part_Price(Guid ID)
        {
            Guid MatID = ID;
            List<Mat_Price_Change> List = IMat.Get_Materil_List_For_Price_Change(MatID, 10);
            return PartialView(List);
        }

    }

}