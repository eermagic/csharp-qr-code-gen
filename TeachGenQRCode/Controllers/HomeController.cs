using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.QrCode;
using static TeachGenQRCode.Models.HomeModel;

namespace TeachGenQRCode.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 產生編碼網址
        /// </summary>
        /// <returns></returns>
        public ActionResult DoUrlEncode(DoUrlEncodeIn inModel)
        {
            DoUrlEncodeOut outModel = new DoUrlEncodeOut();

            // 檢查輸入來源
            if (string.IsNullOrEmpty(inModel.OriText))
            {
                outModel.ErrMsg = "請輸入原始明文";
                return Json(outModel);
            }

            // 將原始明文轉 Base 32 編碼
            string Alphabet = "abcdefghijklmnopqrstuvwxyz234567";
            var valueBytes = Encoding.UTF8.GetBytes(inModel.OriText);
            var encodedBuilder = new StringBuilder();
            var position = 0;
            var left = 0;
            for (var i = 0; i < valueBytes.Length * 8 / 5 + (valueBytes.Length * 8 % 5 == 0 ? 0 : 1); i++)
            {
                var encodedByte = default(byte);
                if (left > 0)
                {
                    encodedByte |= (byte)(valueBytes[position] << (8 - left));
                    if (left <= 5 && position < valueBytes.Length - 1)
                    {
                        position++;
                        if (left < 5) encodedByte |= (byte)(valueBytes[position] >> left);
                    }
                }
                else
                {
                    encodedByte |= valueBytes[position];
                }
                encodedBuilder.Append(Alphabet[(byte)(encodedByte >> 3)]);
                left = 8 * (position + 1) - 5 * (i + 1);
            }

            string base32 = encodedBuilder.ToString();

            // 取得網址起始路徑
            string webPath = Request.Url.Scheme + "://" + Request.Url.Authority;

            // 回傳網址
            outModel.UrlEnText = webPath + Url.Content("~/Home/GetData/?param1=") + base32;

            // 回傳 Json 給前端
            return Json(outModel);
        }

        /// <summary>
        /// 轉成 QR Code
        /// </summary>
        /// <returns></returns>
        public ActionResult DoGenQRCode(DoGenQRCodeIn inModel)
        {
            DoGenQRCodeOut outModel = new DoGenQRCodeOut();

            // 檢查輸入來源
            if (string.IsNullOrEmpty(inModel.UrlEnText))
            {
                outModel.ErrMsg = "請輸入已編碼網址";
                return Json(outModel);
            }

            // BarCode 物件
            BarcodeWriter bw = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions //設定大小
                {
                    Height = 300,
                    Width = 300,
                }
            };

            //產生QR Code
            var img = bw.Write(inModel.UrlEnText); //來源網址
            string FileName = "qrcode.png"; //產生圖檔名稱
            Bitmap myBitmap = new Bitmap(img);
            string FileWebPath = Server.MapPath("~/") + FileName; //完整路徑
            myBitmap.Save(FileWebPath, ImageFormat.Png);
            string QRCodeWebPath = Url.Content("~/") + FileName; // 產生網頁可看到的路徑
            outModel.QRCodeWebPath = QRCodeWebPath;

            // 回傳 Json 給前端
            return Json(outModel);
        }
    }
}