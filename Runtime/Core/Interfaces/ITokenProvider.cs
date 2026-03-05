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
   private string token;

public void SetToken(string newToken)
{
	token = newToken;
}

public string GetToken()
{
	return token;
}
}
