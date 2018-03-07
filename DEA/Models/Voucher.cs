using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEA
{
    public class Voucher
    {
        public string Status { get; set; }
        public string VoucherDescription { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }
        public string Time { get; set; }
        public int Id { get; set; }
        public List<VoucherDetail> VoucherDetail { set; get; }
    }

    public class VoucherDetail
    {
        public string Type { get; set; }
        public string VoucherNo { get; set; }
        public string Time { get; set; }
        public string Code { get; set; }
        public string Transaction { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public double balance { get; set; }

    }
}