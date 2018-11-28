using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace SMART.Api
{
    public class QRCode
    {
        public static string CreateQRCode_Location(string QRString, Guid ID)
        {
            string strCode = string.Empty;
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 10;
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                string Host = HttpRuntime.AppDomainAppPath.ToString();
                string Path = "QRCode\\QRCode_Location\\";

                Image IMG = qrCodeEncoder.Encode(QRString, Encoding.UTF8);
                strCode = Host + Path + ID + ".jpg";
                if (!Directory.Exists(Host + Path)) { Directory.CreateDirectory(Host + Path); }

                FileStream fs = new FileStream(strCode, FileMode.Create, FileAccess.Write);
                IMG.Save(fs, ImageFormat.Jpeg);
                strCode = Path + ID + ".jpg";
                fs.Close();
                IMG.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return strCode;
        }

        //public static string CreateQRCode_Mat(string QRString, Guid ID)
        //{
        //    string strCode = string.Empty;
        //    try
        //    {
        //        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        //        qrCodeEncoder.QRCodeScale = 10;
        //        qrCodeEncoder.QRCodeVersion = 0;
        //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

        //        string Host = HttpRuntime.AppDomainAppPath.ToString();
        //        string Path = "QRCode\\QRCode_Mat\\";

        //        Image IMG = qrCodeEncoder.Encode(QRString, Encoding.UTF8);
        //        strCode = Host + Path + ID + ".jpg";
        //        if (!Directory.Exists(Host + Path)) { Directory.CreateDirectory(Host + Path); }

        //        FileStream fs = new FileStream(strCode, FileMode.Create, FileAccess.Write);
        //        IMG.Save(fs, ImageFormat.Jpeg);
        //        strCode = Path + ID + ".jpg";
        //        fs.Close();
        //        IMG.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //    return strCode;
        //}

        public static string CreateQRCode(string QRString, Guid ID)
        {
            string strCode = string.Empty;
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 10;
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                string Host = HttpRuntime.AppDomainAppPath.ToString();
                string Path = "QRCode_Temp\\";

                Image IMG = qrCodeEncoder.Encode(QRString, Encoding.UTF8);
                strCode = Host + Path + ID + ".jpg";
                if (!Directory.Exists(Host + Path)) { Directory.CreateDirectory(Host + Path); }

                FileStream fs = new FileStream(strCode, FileMode.Create, FileAccess.Write);
                IMG.Save(fs, ImageFormat.Jpeg);
                strCode = Path + ID + ".jpg";
                fs.Close();
                IMG.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return strCode;
        }

        public static string CreateQRCode_With_No(string QRString, Guid ID)
        {
            string strCode = string.Empty;
            try
            {

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 10;
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                string Host = HttpRuntime.AppDomainAppPath.ToString();
                string Path = "QRCode_No_Temp\\";

                Image IMG = qrCodeEncoder.Encode(QRString, Encoding.UTF8);
                strCode = Host + Path + ID + "no.jpg";
                if (!Directory.Exists(Host + Path)) { Directory.CreateDirectory(Host + Path); }

                FileStream fs = new FileStream(strCode, FileMode.Create, FileAccess.Write);
                IMG.Save(fs, ImageFormat.Jpeg);
                strCode = Path + ID + "no.jpg";
                fs.Close();
                IMG.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return strCode;
        }
    }
}
