using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.ModelMapping
{
    public class AutoMapperMapping: BaseMapping
    {
        public AutoMapperMapping(IEnumerable<BaseProfile> profiles)
        {
            if (profiles != null && profiles.Any())
            {
                Mapper.Initialize(x =>
                {
                    foreach (var item in profiles)
                    {
                        x.AddProfile(item);
                    }
                });
            }
        }

        public override TDest Map<TDest, TSource>(TSource source)
        {
            return Mapper.Map<TDest>(source);
        }

        public override TDest Map<TDest, TSource>(TSource source, TDest destination)
        {
            return Mapper.Map<TSource, TDest>(source, destination);
        }
    }
}
