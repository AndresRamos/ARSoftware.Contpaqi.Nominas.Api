using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Sync.Core.Application.Common.Extensions;

public static class ApiResponseExtensions
{
    public static void ThrowIfError(this ApiResponse reponse)
    {
        if (reponse.IsSuccess == false)
            throw new Exception(reponse.ErrorMessage);
    }
}
