using System;

namespace WCA.Core.Features.GlobalX
{
    public static partial class DisbursementFactory
    {
        public struct NonGstTaxCode : ITaxCode, IEquatable<NonGstTaxCode>
        {
            public int TaxCode { get; set; }
            public override bool Equals(object obj) => TaxCode.Equals(obj);
            public override int GetHashCode() => TaxCode.GetHashCode();
            public static bool operator ==(NonGstTaxCode left, NonGstTaxCode right) => left.Equals(right);
            public static bool operator !=(NonGstTaxCode left, NonGstTaxCode right) => !(left == right);
            public bool Equals(NonGstTaxCode other) => TaxCode.Equals(other);
            public static implicit operator NonGstTaxCode(int value) => new NonGstTaxCode { TaxCode = value };
            public NonGstTaxCode ToNonGstTaxCode() => this;
        }
    }
}