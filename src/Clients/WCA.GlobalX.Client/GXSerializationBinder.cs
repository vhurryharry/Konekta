
using Newtonsoft.Json.Serialization;
using System;

namespace WCA.GlobalX.Client
{
    public class GXSerializationBinder : ISerializationBinder
    {
        readonly ISerializationBinder binder;

        public GXSerializationBinder() : this(new DefaultSerializationBinder()) { }

        public GXSerializationBinder(ISerializationBinder binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            this.binder = binder;
        }

        #region ISerializationBinder Members

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            binder.BindToName(serializedType, out assemblyName, out typeName);

            if(typeName != null)
            {
                typeName = typeName.Replace("WCA.GlobalX.Client", "GlobalX.Common.DataModel", StringComparison.InvariantCultureIgnoreCase);
            }

            if(assemblyName != null)
            {
                assemblyName = "GlobalX.CDM.Common";
            }
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return binder.BindToType(assemblyName, typeName);
        }

        #endregion
    }
}
