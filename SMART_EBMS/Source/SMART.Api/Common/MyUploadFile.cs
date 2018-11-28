using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;
using System.Xml.Linq;

namespace SMART.Api
{

    //文件拷贝
    public class FileMove
    {
        public string MaterilFileCopyAndMoveTo(string SourcePath, string FileType, string MainCIDName)
        {
            string NewPath = string.Empty;
            string ReturnPath = string.Empty;

            //原始路径转换为物理路径
            //最后删除原始图片
            string IsRealPhysicalPath = HttpRuntime.AppDomainAppPath.ToString() + SourcePath;
            IsRealPhysicalPath = IsRealPhysicalPath.Replace(@"/", @"\");

            string FileName = Path.GetFileName(IsRealPhysicalPath);

            //如果存在则进行文件Copy
            if (File.Exists(IsRealPhysicalPath))
            {
                NewPath = HttpRuntime.AppDomainAppPath.ToString() + "UpLoadFile";
                if (FileType == "Img")
                {
                    NewPath = NewPath + "\\" + "MaterialImg" + "\\" + MainCIDName + "\\" + DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    NewPath = NewPath + "\\" + "MaterialFile" + "\\" + MainCIDName + "\\" + DateTime.Now.ToString("yyyyMMdd");
                }

                //检查目录是否存在
                if (Directory.Exists(NewPath) == false)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(NewPath);
                        NewPath = NewPath + "\\" + FileName;
                        System.IO.File.Copy(IsRealPhysicalPath, NewPath, true);

                        //本地路径转换成URL相对路径 
                        string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

                        //去除服务器物理根路径
                        string SaveAsWebRootPath = NewPath.Replace(tmpRootDir, ""); //转换成相对路径
                        //转换斜杠
                        ReturnPath = SaveAsWebRootPath.Replace(@"\", @"/");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                    }
                }
                else {
                    try
                    {
                        NewPath = NewPath + "\\" + FileName;
                        System.IO.File.Copy(IsRealPhysicalPath, NewPath, true);

                        //本地路径转换成URL相对路径 
                        string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

                        //去除服务器物理根路径
                        string SaveAsWebRootPath = NewPath.Replace(tmpRootDir, ""); //转换成相对路径
                        //转换斜杠
                        ReturnPath = SaveAsWebRootPath.Replace(@"\", @"/");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                    }       
                }

                //直接拷贝文件并返回新路径的虚拟路径
            }

            //获取原始文件是否存在
            return ReturnPath;
        }
    }

    //文件上传基类
    public class MyUploadFileBase 
    {
        //上传文件类型验证表
        static public string mimeMapXMLPath = HttpRuntime.AppDomainAppPath.ToString() + "XML_config\\mimeMapList.xml";

        //文件上传处理程序
        protected string UpLoadFileProcess(HttpPostedFileBase PostFile, string SubAppUploadDir, string MimeType)
        {
            string SaveAsWebRootPath = string.Empty;

            int ContentLength = PostFile.ContentLength / 1024;     //unit KB
            string ContentType = PostFile.ContentType;

            

            if (ContentLength <= 0)
            {
                throw new Exception("Did not choose to upload file");
            }


            HttpRuntimeSection gre = new HttpRuntimeSection();
            //执行文件大小判定
            if (ContentLength > gre.MaxRequestLength)
            {
                throw new Exception("Upload file size exceeds the server set");
            }


            //获取MIME合法表进行比对
            if (!IsCheckContentType(ContentType, this.FindMimeList(MimeType)))
            {
                throw new Exception("File type is not valid - " + ContentType);
            }

            //无报错，则执行上传过程

            //创建上传目录
            string uploadPath = this.AutoCreatUpLoadDir(SubAppUploadDir);

            string FileName = Path.GetFileName(PostFile.FileName);   //获取文件名
            string FileExtName = Path.GetExtension(FileName);          //获取文件后缀名
            string strGUID = MyGUID.NewGUID().ToString(); //直接返回字符串类型
            string ChangeFileName = strGUID + FileExtName;     //重写GUID文件名


            //重定义上传文件物理路径
            string SaveAsPath = uploadPath + "\\" + ChangeFileName;

            try
            {
                PostFile.SaveAs(SaveAsPath);
                //本地路径转换成URL相对路径 
                string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

                //去除服务器物理根路径
                SaveAsWebRootPath = SaveAsPath.Replace(tmpRootDir, ""); //转换成相对路径
                //转换斜杠
                SaveAsWebRootPath = SaveAsWebRootPath.Replace(@"\", @"/");

            }
            catch
            {
                throw new Exception(SaveAsPath);
            }
            return SaveAsWebRootPath;
        }

        //自动检测上传目录，并创建后返回物理路径字符串
        public string AutoCreatUpLoadDir(string SubAppUploadDir)
        {
            //获取应用根目录
            string upLoadGlobDirInfo = HttpRuntime.AppDomainAppPath.ToString() + "UpLoadFile";

            //创建对应子文件目录
            string UpLoadFilePath = upLoadGlobDirInfo + "\\" + SubAppUploadDir + "\\" + DateTime.Now.ToString("yyyyMMdd");

            //验证目录是否存在，如不存在则自动进行创建
            if (!this.CheckServerUpLoadPath(UpLoadFilePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(UpLoadFilePath);
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }
            }
            return UpLoadFilePath;
        }

        //检查服务器目录是否存在
        private bool CheckServerUpLoadPath(string ServerUpLoadPath)
        {
            return Directory.Exists(ServerUpLoadPath);
        }


        /////<summary>
        /////判断文件类型是否合法，使用MIME定义列表（MIME的英文全称是"Multipurpose Internet Mail Extensions" 多功能Internet 邮件扩充服务，它是一种多用途网际邮件扩充协议）
        /////</summary>
        /////<param name="ContentType">文件类型</param>
        /////<param name="mimeMapInfoList">合法文件类型表</param>
        /////<returns>返回一个bool值，用于判定是否为合法文件类型</returns>
        private bool IsCheckContentType(string ContentType, List<mimeMapInfo> mimeMapInfoList)
        {
            bool Flag = false;
            if (mimeMapInfoList.Where(m => m.mimeType == ContentType).Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }

        /// <summary>
        /// FindMimeList
        /// </summary>
        /// <param name="XMLPath">XML文档所在目录</param>
        /// <param name="ParentName">父级节点</param>
        /// <returns>返回一个ListList<uimeMapInfo>类型，输出MIME列表信息数据</returns>
        public List<mimeMapInfo> FindMimeList(string ParentName)
        {
            XDocument XMLDOC = XDocument.Load(mimeMapXMLPath);
            var queryResult = (from c in XMLDOC.Descendants("mimeMap") where c.Parent.Name == ParentName select c).ToList();
            List<mimeMapInfo> uimeMapInfoList = new List<mimeMapInfo>();
            foreach (var x in queryResult)
            {
                uimeMapInfoList.Add(new mimeMapInfo { 
                fileExtension = x.Attribute("fileExtension").Value,
                mimeType = x.Attribute("mimeType").Value
                });
            }
            return uimeMapInfoList;
        }

    }

    //一般文件上传
    public class MyNormalUploadFile : MyUploadFileBase
    {
        public string NormalUpLoadFileProcess(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            return this.UpLoadFileProcess(PostFile, SubAppUploadDir, "nonmalFileType");
        }
    }

    //一般图像文件上传
    public class MyImgUploadFile : MyUploadFileBase
    {
        public string ImgUpLoadFileProcess(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            return this.UpLoadFileProcess(PostFile, SubAppUploadDir, "imgFileType");
        }   
    }

    //一般图像文件上传按指定尺寸裁切
    public class MyAlbumImageCropUpload : MyImgUploadFile
    {
        public string CreateImgProcess(HttpPostedFileBase PostFile, double targetWidth, double targetHeight, string SubAppUploadDir)
        {

            string SourceImgPath = this.CreatSourceImg(PostFile, SubAppUploadDir);
            string ThumbImgPathA = this.CreateThumbImg(SourceImgPath, targetWidth, targetHeight, "T");

            //最后删除原始图片
            string PhysicalPath = HttpRuntime.AppDomainAppPath.ToString() + SourceImgPath;
            PhysicalPath = PhysicalPath.Replace(@"/", @"\");

            if (File.Exists(PhysicalPath))
            {
                //如果存在则删除
                File.Delete(PhysicalPath);
            }
            return ThumbImgPathA;
        }


        /// <summary>
        /// Cteate SourceImg
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="SubAppUploadDir"></param>
        /// <returns></returns>
        private string CreatSourceImg(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            return this.ImgUpLoadFileProcess(PostFile, SubAppUploadDir);
        }

        /// <summary>
        /// Cteate ThumbImg
        /// </summary>
        /// <param name="SourceImgPath"></param>
        /// <returns></returns>
        private string CreateThumbImg(string SourceImgPath, double targetWidth, double targetHeight, string lastStr)
        {
            string ThumbImg = string.Empty;
            string originalImgPath = HttpRuntime.AppDomainAppPath.ToString() + SourceImgPath;
            originalImgPath = originalImgPath.Replace(@"/", @"\");

            //截取掉后缀名
            int lastIndexOfDot = originalImgPath.LastIndexOf(".");
            ThumbImg = originalImgPath.Substring(0, lastIndexOfDot).ToString() + "_" + lastStr + ".png";

            FileStream originalImgFile = File.Open(originalImgPath, FileMode.Open, FileAccess.Read, FileShare.None);
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(originalImgFile, true);

            //原始图像尺寸
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;

            //宽大于高或宽等于高（横图或正方）
            if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
            {
                //如果宽大于模版
                if (initImage.Width > targetWidth)
                {
                    //宽按模版，高按比例缩放
                    newWidth = targetWidth;
                    newHeight = initImage.Height * (targetWidth / initImage.Width);
                }
            }
            //高大于宽（竖图）
            else
            {
                //如果高大于模版
                if (initImage.Height > targetHeight)
                {
                    //高按模版，宽按比例缩放
                    newHeight = targetHeight;
                    newWidth = initImage.Width * (targetHeight / initImage.Height);
                }
            }

            //新建一个bmp图片
            System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);

            //新建一个画板
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

            //设置质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //置背景色
            newG.Clear(System.Drawing.Color.White);
            //画图
            newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

            newImage.Save(ThumbImg, System.Drawing.Imaging.ImageFormat.Jpeg);

            //释放原始图片资源
            originalImgFile.Dispose();
            initImage.Dispose();
            newImage.Dispose();

            //本地路径转换成URL相对路径 
            string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

            //去除服务器物理根路径
            string SaveAsWebRootPath = ThumbImg.Replace(tmpRootDir, ""); //转换成相对路径
            //转换斜杠
            SaveAsWebRootPath = SaveAsWebRootPath.Replace(@"\", @"/");

            return SaveAsWebRootPath;
        }


    }

    //一般图像文件上传，具备缩略图功能
    public class MyAlbumImageUploadBasic : MyImgUploadFile
    {
        /// <summary>
        /// 物料主文件图片上传
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="SubAppUploadDir"></param>
        /// <returns>返回字符串数组，用于存储原始图片，相册图片，缩略图图片虚拟路径</returns>
        public string[] CreateImgProcess(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            string SourceImgPath = this.CreatSourceImg(PostFile, SubAppUploadDir);
            string ThumbImgPathA = this.CreateThumbImg(SourceImgPath, 450, 450, "T");

            string[] imgPathArray = new string[2] { SourceImgPath, ThumbImgPathA };

            return imgPathArray;       
        }

        /// <summary>
        /// Cteate SourceImg
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="SubAppUploadDir"></param>
        /// <returns></returns>
        private string CreatSourceImg(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            return this.ImgUpLoadFileProcess(PostFile, SubAppUploadDir);
        }

        /// <summary>
        /// Cteate ThumbImg
        /// </summary>
        /// <param name="SourceImgPath"></param>
        /// <returns></returns>
        private string CreateThumbImg(string SourceImgPath, double targetWidth, double targetHeight, string lastStr)
        {
            string ThumbImg = string.Empty;
            string originalImgPath = HttpRuntime.AppDomainAppPath.ToString() + SourceImgPath;
            originalImgPath = originalImgPath.Replace(@"/", @"\");

            //截取掉后缀名
            int lastIndexOfDot = originalImgPath.LastIndexOf(".");
            ThumbImg = originalImgPath.Substring(0, lastIndexOfDot).ToString() + "_" + lastStr + ".png";

            FileStream originalImgFile = File.Open(originalImgPath, FileMode.Open, FileAccess.Read, FileShare.None);
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(originalImgFile, true);    

            //原始图像尺寸
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;

            //宽大于高或宽等于高（横图或正方）
            if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
            {
                //如果宽大于模版
                if (initImage.Width > targetWidth)
                {
                    //宽按模版，高按比例缩放
                    newWidth = targetWidth;
                    newHeight = initImage.Height * (targetWidth / initImage.Width);
                }
            }
            //高大于宽（竖图）
            else
            {
                //如果高大于模版
                if (initImage.Height > targetHeight)
                {
                    //高按模版，宽按比例缩放
                    newHeight = targetHeight;
                    newWidth = initImage.Width * (targetHeight / initImage.Height);
                }
            }

            //新建一个bmp图片
            System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);

            //新建一个画板
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

            //设置质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //置背景色
            newG.Clear(System.Drawing.Color.White);
            //画图
            newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

            newImage.Save(ThumbImg, System.Drawing.Imaging.ImageFormat.Png);

            //释放原始图片资源
            originalImgFile.Dispose();
            initImage.Dispose();
            newImage.Dispose();

            //本地路径转换成URL相对路径 
            string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

            //去除服务器物理根路径
            string SaveAsWebRootPath = ThumbImg.Replace(tmpRootDir, ""); //转换成相对路径
            //转换斜杠
            SaveAsWebRootPath = SaveAsWebRootPath.Replace(@"\", @"/");

            return SaveAsWebRootPath;
        }

    }

    //产品相册图片上传，自动裁切缩略图
    public class MyAlbumImageUpload : MyImgUploadFile
    {
        /// <summary>
        /// 物料主文件图片上传
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="SubAppUploadDir"></param>
        /// <returns>返回字符串数组，用于存储原始图片，相册图片，缩略图图片虚拟路径</returns>
        public string[] CreateMatImgProcess(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            string SourceImgPath = this.CreatSourceImg(PostFile, SubAppUploadDir);
            string ThumbImgPathA = this.CreateThumbImg(SourceImgPath, 400, 400, "T");
            string ThumbImgPathB = this.CreateThumbImg(SourceImgPath, 120, 120,"S");

            string[] imgPathArray = new string[3] { SourceImgPath, ThumbImgPathA, ThumbImgPathB };

            return imgPathArray;
        }

        /// <summary>
        /// Cteate SourceImg
        /// </summary>
        /// <param name="PostFile"></param>
        /// <param name="SubAppUploadDir"></param>
        /// <returns></returns>
        private string CreatSourceImg(HttpPostedFileBase PostFile, string SubAppUploadDir)
        {
            return this.ImgUpLoadFileProcess(PostFile, SubAppUploadDir);
        }

        /// <summary>
        /// Cteate ThumbImg
        /// </summary>
        /// <param name="SourceImgPath"></param>
        /// <returns></returns>
        private string CreateThumbImg(string SourceImgPath, double targetWidth, double targetHeight, string lastStr)
        {
            string ThumbImg = string.Empty;
            string originalImgPath = HttpRuntime.AppDomainAppPath.ToString() + SourceImgPath;
            originalImgPath = originalImgPath.Replace(@"/", @"\");

            //截取掉后缀名
            int lastIndexOfDot = originalImgPath.LastIndexOf(".");
            ThumbImg = originalImgPath.Substring(0, lastIndexOfDot).ToString() + "_" + lastStr + ".png";

            FileStream originalImgFile = File.Open(originalImgPath, FileMode.Open, FileAccess.Read, FileShare.None);
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(originalImgFile, true);

            //原始图像尺寸
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;

            //宽大于高或宽等于高（横图或正方）
            if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
            {
                //如果宽大于模版
                if (initImage.Width > targetWidth)
                {
                    //宽按模版，高按比例缩放
                    newWidth = targetWidth;
                    newHeight = initImage.Height * (targetWidth / initImage.Width);
                }
            }
            //高大于宽（竖图）
            else
            {
                //如果高大于模版
                if (initImage.Height > targetHeight)
                {
                    //高按模版，宽按比例缩放
                    newHeight = targetHeight;
                    newWidth = initImage.Width * (targetHeight / initImage.Height);
                }
            }

            //新建一个bmp图片
            System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);

            //新建一个画板
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

            //设置质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //置背景色
            newG.Clear(System.Drawing.Color.White);
            //画图
            newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

            newImage.Save(ThumbImg, System.Drawing.Imaging.ImageFormat.Png);
            
            //释放原始图片资源
            originalImgFile.Dispose();
            initImage.Dispose();
            newImage.Dispose();

            //本地路径转换成URL相对路径 
            string tmpRootDir = HttpRuntime.AppDomainAppPath.ToString();//获取应用根目录

            //去除服务器物理根路径
            string SaveAsWebRootPath = ThumbImg.Replace(tmpRootDir, ""); //转换成相对路径
            //转换斜杠
            SaveAsWebRootPath = SaveAsWebRootPath.Replace(@"\", @"/");

            return SaveAsWebRootPath;
        }
    }

    //文件删除类
    public class MyDeleteUploadFile
    {
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="visPath"></param>
        static public void MyDeleteUploadFileProcess(string visPath)
        {
            //执行虚拟路径转换为物理路径
            string PhysicalPath = HttpRuntime.AppDomainAppPath.ToString() + visPath;
            PhysicalPath = PhysicalPath.Replace(@"/", @"\");

            if (File.Exists(PhysicalPath))
            {
                //如果存在则删除
                File.Delete(PhysicalPath);
            }
        }
    }


    /// <summary>
    /// mimeMapInfo
    /// </summary>
    public class mimeMapInfo
    {
        //文件扩展名
        public string fileExtension { get; set; }
        //文件MimeType
        public string mimeType { get; set; }
    }
}