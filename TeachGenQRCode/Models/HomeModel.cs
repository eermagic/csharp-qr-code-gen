using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachGenQRCode.Models
{
    public class HomeModel
    {
        /// <summary>
        /// [產生編碼網址]參數
        /// </summary>
        public class DoUrlEncodeIn
        {
            public string OriText { get; set; }
        }

        /// <summary>
        /// [產生編碼網址]回傳
        /// </summary>
        public class DoUrlEncodeOut
        {
            public string ErrMsg { get; set; }
            public string UrlEnText { get; set; }
        }

        /// <summary>
        /// [轉成 QR Code]參數
        /// </summary>
        public class DoGenQRCodeIn
        {
            public string UrlEnText { get; set; }
        }

        /// <summary>
        /// [轉成 QR Code]回傳
        /// </summary>
        public class DoGenQRCodeOut
        {
            public string ErrMsg { get; set; }
            public string QRCodeWebPath { get; set; }
        }
    }
}