using System;

namespace WCA.Core.Features.GlobalX
{
    public static partial class DisbursementFactory
    {
        public struct GstTaxCode : ITaxCode, IEquatable<GstTaxCode>
        {
            public int TaxCode { get; set; }
            public override bool Equals(object obj) => TaxCode.Equals(obj);
            public override int GetHashCode() => TaxCode.GetHashCode();
            public static bool operator ==(GstTaxCode left, GstTaxCode right) => left.Equals(right);
            public static bool operator !=(GstTaxCode left, GstTaxCode right) => !(left == right);
            public bool Equals(GstTaxCode other) => TaxCode.Equals(other);
            public static implicit operator GstTaxCode(int value) => new GstTaxCode { TaxCode = value };
            public GstTaxCode ToGstTaxCode() => this;
        }
    }
}