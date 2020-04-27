using System.ComponentModel.DataAnnotations;

namespace WCA.Actionstep.Client.Contracts
{
    public enum ActionstepDataType
    {
        [Display(Name = "Text Line")]
        Str255,

        [Display(Name = "Protected Text Line")]
        Str255Protected,

        [Display(Name = "Text Block")]
        TextArea,

        [Display(Name = "Number")]
        Number,

        [Display(Name = "Money")]
        Money,

        [Display(Name = "Date")]
        DateNM,

        [Display(Name = "Date and Time")]
        DateTimeNM,

        [Display(Name = "Calendar Appointment")]
        Appointment,

        [Display(Name = "Dropdown List (Single Select")]
        DropDown,

        [Display(Name = "Dropdown List (Multi Select")]
        DropDownMulti,

        [Display(Name = "Auto Number (Global")]
        AutoNumberDC,

        [Display(Name = "Auto Number (Action")]
        AutoNumber,

        [Display(Name = "Boolean (Checkbox")]
        Boolean,

        [Display(Name = "Date (with memo")]
        Date,

        [Display(Name = "Date and Time (with memo")]
        DateTime,

        [Display(Name = "Linked Participant")]
        Participant,

        [Display(Name = "HTML Block (no editable value")]
        HtmlReadOnly,
    }
}
