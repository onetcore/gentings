using Microsoft.Extensions.Hosting;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    /// <summary>
    /// 后台服务列表。
    /// </summary>
    public class BackgroundServiceModel : ModelBase
    {
        private readonly IEnumerable<IHostedService> _hostedServices;

        /// <summary>
        /// 初始化类<see cref="BackgroundServiceModel"/>。
        /// </summary>
        /// <param name="hostedServices">后台服务接口列表。</param>
        public BackgroundServiceModel(IEnumerable<IHostedService> hostedServices)
        {
            _hostedServices = hostedServices;
        }

        /// <summary>
        /// 后台服务进程。
        /// </summary>
        public IEnumerable<Gentings.Tasks.BackgroundService> HostedServices { get; private set; }

        /// <summary>
        /// 获取后台服务。
        /// </summary>
        /// <param name="orderBy">排序规则。</param>
        public void OnGet(BackgroundServiceOrderBy orderBy)
        {
            var services = _hostedServices
                .Select(x => x as Gentings.Tasks.BackgroundService)
                .Where(x => x != null);
            if (orderBy != null)
            {
                switch (orderBy.Order)
                {
                    case BackgroundServiceOrder.RunningTime:
                        services = orderBy.Sorted(services, x => x.RunningTime);
                        break;
                    case BackgroundServiceOrder.StartDate:
                        services = orderBy.Sorted(services, x => x.StartDate);
                        break;
                }
            }
            HostedServices = services.ToList();
        }
    }
}