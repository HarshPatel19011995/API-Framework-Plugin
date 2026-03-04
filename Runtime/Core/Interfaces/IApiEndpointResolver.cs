/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *
 *  Description :
 *  Generic endpoint resolver interface.
 *  Implemented by each project using the SDK.
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Interfaces
{
    public interface IApiEndpointResolver
    {
        string GetFullUrl(string endpointKey);
    }
}