using System.Text;
using Newtonsoft.Json;
using ToDoer.Api.Domain;

namespace ToDoer.Api.Models
{
    public class TaskCreateCommand
    {
        public string ListId {get;set;}
        public string Description {get;set;}
        public string Subject {get;set;}

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TaskCreateCommand {\n");
            sb.Append("  ListId: ").Append(ListId).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Subject: ").Append(Subject).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TaskCreateCommand)obj);
        }

        /// <summary>
        /// Returns true if ListDetails instances are equal
        /// </summary>
        /// <param name="other">Instance of ListDetails to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TaskCreateCommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ListId == other.ListId ||
                    ListId != null &&
                    ListId.Equals(other.ListId)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    Subject == other.Subject ||
                    Subject != null &&
                    Subject.Equals(other.Subject)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (ListId != null)
                    hashCode = hashCode * 59 + ListId.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (Subject != null)
                    hashCode = hashCode * 59 + Subject.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(TaskCreateCommand left, ListDetails right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TaskCreateCommand left, ListDetails right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}