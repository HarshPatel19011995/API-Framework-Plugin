/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *
 *  Description :
 *  Abstraction for providing authentication tokens.
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Interfaces
{
    public interface ITokenProvider
    {
        string GetToken();
    }
}