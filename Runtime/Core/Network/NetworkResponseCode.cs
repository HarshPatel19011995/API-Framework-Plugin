/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Network
{
    public enum NetworkResponseCode
    {
        None = 0,
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        InternalServerError = 500,
        Timeout = 408
    }
}