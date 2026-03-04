using System.Collections.Generic;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    public class ApiMiddlewarePipeline
    {
        private readonly List<IApiMiddleware> middlewares = new();

        public void Use(IApiMiddleware middleware)
        {
            middlewares.Add(middleware);
        }

        public MiddlewareDelegate Build(MiddlewareDelegate terminal)
        {
            MiddlewareDelegate current = terminal;

            for (int i = middlewares.Count - 1; i >= 0; i--)
            {
                var middleware = middlewares[i];
                var next = current;

                current = (request) =>
                    middleware.InvokeAsync(request, next);
            }

            return current;
        }
    }
}