using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;//for model metadata

namespace JobBoardFinalProject.DATA.EF//.Metadata
{
    #region ApplicationMetadata
    public class ApplicationMetadata
    {
        [Required(ErrorMessage = "*User ID is required")]
        [Display(Name = "User ID (email address)")]
        [EmailAddress]
        public string UserId { get; set; }

        [Required(ErrorMessage = "*Open Position ID is required")]
        [Display(Name = "Open Position ID")]
        public int OpenPositionId { get; set; }

        [Required(ErrorMessage ="*Application Date is required")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "Date unknown")]
        [Display(Name = "Application Date")]
        public System.DateTime ApplicationDate { get; set; }

        [UIHint("MultilineText")]
        [StringLength(200, ErrorMessage = "*Value must be 200 characters or less")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Manager Notes")]
        public string ManagerNotes { get; set; }

        [Required(ErrorMessage ="*Application Status ID is required")]
        [Display(Name = "Application Status ID")]
        public int ApplicationStatusId { get; set; }

        [Required(ErrorMessage ="*Resume is required")]
        [Display(Name = "Resume")]
        public string ResumeFilename { get; set; }
    }

    [MetadataType(typeof(ApplicationMetadata))]
    public partial class Application { }

    #endregion

    #region ApplicationStatusMetadata
    public class ApplicationStatusMetadata
    {
        [Required(ErrorMessage ="*Application Status Name is required")]
        [StringLength(50, ErrorMessage="*Value must be 50 characters or less")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }

        [UIHint("MultilineText")]
        [StringLength(250, ErrorMessage = "*Value must be 250 characters or less")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Status Description")]
        public string StatusDescription { get; set; }
    }

    [MetadataType(typeof(ApplicationStatusMetadata))]
    public partial class ApplicationStatus { }

    #endregion

    #region UserDetailsMetadata
    public class UserDetailMetadata
    {
        [Required(ErrorMessage ="*First Name is required")]
        [StringLength(50, ErrorMessage="*Value must be 50 characters or less")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="*Last Name is required")]
        [StringLength(50, ErrorMessage = "*Value must be 50 characters or less")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Resume")]
        [StringLength(75, ErrorMessage="*Value must be 75 characters or less")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string ResumeFilename { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        [Display(Name = "Employee Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    #endregion

    #region OpenPositionMetadata
    public class OpenPositionMetadata
    {
        [Required(ErrorMessage ="*Location ID is required")]
        [Display(Name = "Location ID")]
        public int LocationId { get; set; }

        [Required(ErrorMessage ="*Position ID is required")]
        [Display(Name = "Position ID")]
        public int PositionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "Date unknown")]
        [Display(Name = "Posting Date")]
        public System.DateTime PostingDate { get; set; }
    }

    [MetadataType(typeof(OpenPositionMetadata))]
    public partial class OpenPosition { }

    #endregion

    #region LocationMetadata
    public class LocationMetadata
    {
        [Required(ErrorMessage ="*Branch Number is required")]
        [StringLength(10, ErrorMessage="*Value must be 10 characters or less")]
        [Display(Name = "Branch Number")]
        public string BranchNumber { get; set; }

        [StringLength(225, ErrorMessage="*Value must be 225 characters or less")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage ="*City is required")]
        [StringLength(50, ErrorMessage="*Value must be 50 characters or less")]
        public string City { get; set; }

        [Required(ErrorMessage = "*State is required")]
        [StringLength(50, ErrorMessage = "*Value must be 2-letter state code")]
        public string State { get; set; }

        [StringLength(11, ErrorMessage="*Value must be 11 characters or less")]
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage ="*Manager ID is required")]
        [Display(Name = "Manager ID")]
        public string ManagerId { get; set; }
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }

    #endregion

    #region PositionMetadata
    public class PositionMetadata
    {
        [Required(ErrorMessage ="*Title is required")]
        [StringLength(50, ErrorMessage="*Value must be 50 characters or less")]
        public string Title { get; set; }

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage ="*Department ID is required")]
        [Display(Name = "Department ID")]
        public int DepartmentId { get; set; }

        [Display(Name = "Relocation Benefits?")]
        public bool HasRelocationBenefits { get; set; }

        [Display(Name = "Full Time?")]
        public bool IsFullTime { get; set; }

        [Display(Name = "Salary Position?")]
        public bool IsSalary { get; set; }

        [Display(Name = "Supervisory Responsibilities?")]
        public bool HasSupervisoryResponsibilities { get; set; }

        [Display(Name = "Featured Position?")]
        public bool IsFeaturedPosition { get; set; }
    }

    [MetadataType(typeof(PositionMetadata))]
    public partial class Position { }
    #endregion

    #region DepartmentMetadata
    public class DepartmentMetadata
    {
        [Required(ErrorMessage ="*Department Name is required")]
        [StringLength(100, ErrorMessage="*Value must be 100 characters or less")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department { }
    #endregion
}
