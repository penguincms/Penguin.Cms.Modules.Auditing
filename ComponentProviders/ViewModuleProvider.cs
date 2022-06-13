using Penguin.Cms.Abstractions.Interfaces;
using Penguin.Cms.Auditing.Repositories;
using Penguin.Cms.Entities;
using Penguin.Cms.Modules.Dynamic.Areas.Admin.Models;
using Penguin.Cms.Web.Modules;
using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Extensions.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Cms.Modules.Auditing.ComponentProviders
{
    public class ViewModuleProvider : IProvideComponents<ViewModule, Entity>
    {
        protected AuditEntryRepository AuditEntryRepository { get; set; }

        public ViewModuleProvider(AuditEntryRepository auditEntryRepository)
        {
            this.AuditEntryRepository = auditEntryRepository;
        }

        public IEnumerable<ViewModule> GetComponents(Entity Id)
        {
            yield return new ViewModule("~/Areas/Admin/Views/Render/List.cshtml", new DynamicListModel<IMetaObject>(AuditEntryRepository.Where(a => a.Target == Id.Guid).OrderByDescending(a => a.DateCreated).ToList().ToMetaList(true)), "Audit Entries");
        }
    }
}