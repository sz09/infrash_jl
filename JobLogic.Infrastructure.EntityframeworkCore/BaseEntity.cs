using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.EntityframeworkCore
{
    public abstract class BaseEntity
    {
        #region Constructors
        public BaseEntity() { }
        #endregion

        #region Properties
        [Column("Deleted")]
        public bool IsDeleted { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        #region Navigation Properties
        #endregion
        #endregion

        #region Methods
        public virtual IEnumerable<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), results, false);
            return results;
        }

        public bool isValid()
        {
            return !Validate().Any();
        }

        public virtual void Remove()
        {
            if (CanDeleteEntity)
                IsDeleted = true;
        }

        private bool _CanDeleteEntity = true;
        [NotMapped]
        public bool CanDeleteEntity { get { return _CanDeleteEntity; } set { _CanDeleteEntity = value; } }
        #endregion

    }
}
