using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace SMART.Api
{
    public class MyDbSave
    {
        public static void SaveChange(SmartdbContext db)
        {
            try { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                string ErrorList = string.Empty;
                foreach (var x in ex.EntityValidationErrors)
                {
                    foreach (var xx in x.ValidationErrors)
                    {
                        ErrorList += xx.ErrorMessage.ToString();
                    }
                }
                throw new Exception(ErrorList);
            }
        }
    }
}