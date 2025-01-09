using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Domain
{

    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class Entity : CleanCode, IValidatableObject
    {
        #region Members

        int? _requestedHashCode;
        Guid _Id;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the persisten object identifier
        /// </summary>
        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;

                OnIdChanged();
            }
        }

        public DateTime CreatedOn { get; protected set; }
        public DateTime UpdatedOn { get; protected set; }

        public void SetCreatedOn()
        {
            CreatedOn = DateTime.UtcNow;
        }
        public void SetUpdatedOn()
        {
            UpdatedOn = DateTime.UtcNow;
        }


        #endregion

        #region Abstract Methods

        /// <summary>
        /// When POID is changed
        /// </summary>
        protected virtual void OnIdChanged()
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return this.Id == Guid.Empty;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ValidateEntity(validationContext);
        }

        public void GaurdForValidationErrors()
        {
            var errors = ValidateEntity(null).ToList().Select(r => r.ErrorMessage);
            if (errors.Any())
                throw new ValidationException(string.Join(" ", errors));
        }

        public abstract IEnumerable<ValidationResult> ValidateEntity(ValidationContext validationContext);

        public void GaurdForDelete()
        {
            throw new ValidationException("InvalidOperationNoDeleteGaurdFoundForEntity");
        }

        protected Guid? ReplaceEmptyOrNullWithNull(Guid? input)
        {
            if (!input.HasValue) return null;
            if (input.HasValue && input.Value == Guid.Empty) return null;
            return input;
        }

        public static int GetPastDateDifferenceInDaysFromToDay(DateTime pastDate)
        {
            return Convert.ToInt32((DateTime.UtcNow - pastDate).TotalDays);
        }

        public static bool IsDateOlderThan(int days, DateTime pastDate)
        {
            var howOldIsDateInDays = GetPastDateDifferenceInDaysFromToDay(pastDate);
            return howOldIsDateInDays >= days;
        }
        public static bool IsPastDate(DateTime pastDateUtc)
        {
            return DateTime.UtcNow > pastDateUtc;
        }

        protected Entity() { }

    }
}
