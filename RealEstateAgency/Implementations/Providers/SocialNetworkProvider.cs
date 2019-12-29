using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RealEstateAgency.DAL.Models;
using RealEstateAgency.Dtos.ModelDtos.SharingSystem;

namespace RealEstateAgency.Implementations.Providers
{
    public class SocialNetworkProvider : ISocialNetworkProvider
    {
        public SocialNetworkProvider(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<RealEstateDbContext>();
                var languages = context.SocialNetwork.ToList();
                SocialNetworks = new Dictionary<string, SocialNetworkDto>();
                languages.Select(i => new SocialNetworkDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    UniqueKey = i.UniqueKey
                }).ToList().ForEach(i => SocialNetworks.Add(i.UniqueKey, i));
            }
        }
        public Dictionary<string, SocialNetworkDto> SocialNetworks { get; }

        public SocialNetworkDto this[string code] => SocialNetworks[code];
    }

    public interface ISocialNetworkProvider
    {
        Dictionary<string, SocialNetworkDto> SocialNetworks { get; }
        SocialNetworkDto this[string code] { get; }
    }
}
