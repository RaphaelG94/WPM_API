﻿using System;
using System.Collections.Generic;
using System.Linq;
using WPM_API.Data.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WPM_API.Code.Infrastructure
{
    public class ViewDataItems
    {
        private UnitOfWork UnitOfWork { get; }
        private Dictionary<string, object> Cache { get; }

        public ViewDataItems(IUnitOfWorkFactory unitOfWorkFactory)
        {
            UnitOfWork = unitOfWorkFactory.UnitOfWork;
            Cache = new Dictionary<string, object>();
        }

        public List<SelectListItem> Roles
        {
            get
            {
                return GetOrAdd("Roles",
                    () => UnitOfWork.Users.Roles.GetAllRoles().Select(x=>new SelectListItem() { Value = x.Id.ToString(), Text = x.Name}).ToList());
            }
        }

        private T GetOrAdd<T>(string key, Func<T> loadAction)
        {
            T res;
            object obj;
            if (Cache.TryGetValue(key, out obj))
            {
                res = (T)obj;
            }
            else
            {
                res = loadAction();
                Cache.Add(key, res);
            }
            return res;
        }

    }
}
