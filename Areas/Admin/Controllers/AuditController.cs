using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Auditing;
using Penguin.Cms.Auditing.Repositories;
using Penguin.Cms.Modules.Admin.Areas.Admin.Controllers;
using Penguin.Cms.Modules.Core.Models;
using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Constructors;
using Penguin.Reflection.Serialization.Objects;
using System;
using System.Linq;

namespace Penguin.Cms.Modules.Auditing.Areas.Admin.Controllers
{
    public class AuditController : AdminController
    {
        protected AuditEntryRepository AuditEntryRepository { get; set; }

        public AuditController(AuditEntryRepository auditEntryRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            AuditEntryRepository = auditEntryRepository;
        }

        public ActionResult ListEntries(string TypeName = "", Guid? Target = null, int count = 20, int page = 0)
        {
            bool typeSelected = !string.IsNullOrWhiteSpace(TypeName);

            IQueryable<AuditEntry> FilterQuery(IQueryable<AuditEntry> toQuery)
            {
                return toQuery.Where(a => (!typeSelected || a.TypeName == TypeName) && (Target == null || a.Target == Target));
            }

            MetaConstructor c = Constructor;

            PagedListContainer<IMetaObject> model = new PagedListContainer<IMetaObject>
            {
                TotalCount = FilterQuery(AuditEntryRepository.All).Count(),
                Page = page,
                Count = count
            };

            model.Items.AddRange(FilterQuery(AuditEntryRepository.All).OrderByDescending(a => a.Logged).Skip(page * count).Take(count).ToList().Select(o => { IMetaObject me = new MetaObject(o, c); me.Hydrate(); return me; }));

            if (Target.HasValue)
            {
                model.HiddenColumns.Add(nameof(Target));
                model.HiddenColumns.Add(nameof(TypeName));
            }
            else if (!string.IsNullOrWhiteSpace(TypeName))
            {
                model.HiddenColumns.Add(nameof(TypeName));
            }

            return this.View(model);
        }
    }
}