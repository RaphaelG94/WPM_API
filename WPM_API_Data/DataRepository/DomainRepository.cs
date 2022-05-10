using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class DomainRepository : RepositoryEntityDeletableBase<Domain>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DomainRepository(DataContextProvider context) : base(context)
        {
        }

        public List<OrganizationalUnit> GetOrganisationalUnits(string domainId)
        {
            List<OrganizationalUnit> ous = Context.Set<Domain>().Find(domainId).OrganizationalUnits.ToList();
            foreach(var ou in ous)
            {
                var children = GetChildren(ou);
                if(children != null)
                {
                    ou.Children = children;
                }
            }
            return ous;
        }

        private List<OrganizationalUnit> GetChildren(OrganizationalUnit ou)
        {
            List<OrganizationalUnit> children = null;
            OrganizationalUnit o = Context.Set<OrganizationalUnit>().Include("Children").Single(x => x.Id.Equals(ou.Id));
            if (o.Children != null)
            {
                children = o.Children.ToList();
                foreach (var child in children)
                {
                    var grandchildren = GetChildren(child);
                    if (grandchildren != null)
                    {
                        child.Children = grandchildren;
                    }
                }
            }
            return children;
        }
    }
}