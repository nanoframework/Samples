//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AzureIoTExample
{
    
    internal partial class Resource
    {
        private static System.Resources.ResourceManager manager;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if ((Resource.manager == null))
                {
                    Resource.manager = new System.Resources.ResourceManager("AzureIoTExample.Resource", typeof(Resource).Assembly);
                }
                return Resource.manager;
            }
        }
        internal static string GetString(Resource.StringResources id)
        {
            return ((string)(nanoFramework.Runtime.Native.ResourceUtility.GetObject(ResourceManager, id)));
        }
        [System.SerializableAttribute()]
        internal enum StringResources : short
        {
            AzureRootCerts = -6742,
        }
    }
}
