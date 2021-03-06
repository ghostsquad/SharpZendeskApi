﻿// .\GetZendeskModelDataFromUrl.ps1 http://developer.zendesk.com/documentation/rest_api/users.html
// End users are represented as JSON objects which have the following keys:
// Name            Type       ReadOnly Mandatory Comment
// ----            ----       -------- --------- -------
// created_at      date       yes      no        The time the user was created
// email           string     no       yes       The primary email address of this user
// id              integer    yes      no        Automatically assigned when creating users
// locale          string     yes      no        The locale for this user
// locale_id       integer    no       no        The language identifier for this user
// name            string     no       yes       The name of the user
// organization_id integer    no       no        The id of the organization this user is associated with
// phone           string     no       no        The primary phone number of this user
// photo           Attachment no       no        The user's profile picture represented as an Attachment object
// role            string     no       yes       The role of the user. Possible values: "end-user", "agent", "admin"
// time_zone       string     no       no        The time-zone of this user
// updated_at      date       yes      no        The time of the last update of the user
// url             string     yes      no        The API url of this user
// verified        boolean    no       no        Zendesk has verified that this user is who he says he is
// 
// Users are represented as JSON objects which have the following keys:
// Name                  Type       ReadOnly Mandatory Comment
// ----                  ----       -------- --------- -------
// active                boolean    yes      no        Users that have been deleted will have the value false here
// alias                 string     no       no        Agents can have an alias that is displayed to end-users
// created_at            date       yes      no        The time the user was created
// custom_role_id        integer    no       no        A custom role on the user if the user is an agent on the entreprise plan
// details               string     no       no        In this field you can store any details obout the user. e.g. the address
// email                 string     no       yes       The primary email address of this user
// external_id           string     no       no        A unique id you can set on a user
// id                    integer    yes      no        Automatically assigned when creating users
// last_login_at         date       yes      no        A time-stamp of the last time this user logged in to Zendesk
// locale                string     yes      no        The locale for this user
// locale_id             integer    no       no        The language identifier for this user
// moderator             boolean    no       no        Designates whether this user has forum moderation capabilities
// name                  string     no       yes       The name of the user
// notes                 string     no       no        In this field you can store any notes you have about the user
// only_private_comments boolean    no       no        true if this user only can create private comments
// organization_id       integer    no       no        The id of the organization this user is associated with
// phone                 string     no       no        The primary phone number of this user
// photo                 Attachment no       no        The user's profile picture represented as an Attachment object
// restricted_agent      boolean    no       no        Indicates whether agent has any restrictions; false for admins and unrestricted agents, true for
//                                                     other agents
// role                  string     no       yes       The role of the user. Possible values: "end-user", "agent", "admin"
// shared                boolean    yes      no        If this user is shared from a different Zendesk, ticket sharing accounts only
// shared_agent          boolean    yes      no        If this user is a shared agent from a different Zendesk, ticket sharing accounts only
// signature             string     no       no        The signature of this user. Only agents and admins can have signatures
// suspended             boolean    no       no        Tickets from suspended users are also suspended, and these users cannot log in to the end-user
//                                                     portal
// tags                  array      no       no        The tags of the user. Only present if your account has user tagging enabled
// ticket_restriction    string     no       no        Specified which tickets this user has access to. Possible values are: "organization", "groups",
//                                                     "assigned", "requested", null
// time_zone             string     no       no        The time-zone of this user
// updated_at            date       yes      no        The time of the last update of the user
// url                   string     yes      no        The API url of this user
// user_fields           hash       no       no        Custom fields for this user
// verified              boolean    no       no        Zendesk has verified that this user is who he says he is
namespace SharpZendeskApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using SharpZendeskApi.Models.Attributes;

    /// <summary>
    /// The user.
    /// </summary>
    public class User : TrackableZendeskThingBase, IUser
    {
        #region Fields

        private string alias;

        private int? customRoleId;

        private string details;

        private string email;

        private string externalId;

        private int? localeId;

        private bool? moderator;

        private string name;

        private string notes;

        private bool? onlyPrivateComments;

        private int? organizationId;

        private string phone;

        private IAttachment photo;

        private bool? restrictedAgent;

        private string role;

        private string signature;

        private bool? suspended;

        private string[] tags;

        private string ticketRestriction;

        private string timeZone;

        private CustomField[] userFields;

        private bool? verified;

        #endregion

        #region Constructors and Destructors

        public User(string email, string name, string role)
        {
            this.Email = email;
            this.Name = name;
            this.Role = role;
        }

        [InjectionConstructor]
        public User()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether active.
        /// </summary>
        [ReadOnly]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias
        {
            get
            {
                return this.alias;
            }

            set
            {
                this.alias = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        [ReadOnly]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the custom role id.
        /// </summary>
        public int? CustomRoleId
        {
            get
            {
                return this.customRoleId;
            }

            set
            {
                this.customRoleId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        public string Details
        {
            get
            {
                return this.details;
            }

            set
            {
                this.details = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Mandatory]
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                this.email = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the external id.
        /// </summary>
        public string ExternalId
        {
            get
            {
                return this.externalId;
            }

            set
            {
                this.externalId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the last login at.
        /// </summary>
        [ReadOnly]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        [ReadOnly]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the locale id.
        /// </summary>
        public int? LocaleId
        {
            get
            {
                return this.localeId;
            }

            set
            {
                this.localeId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether moderator.
        /// </summary>
        public bool? Moderator
        {
            get
            {
                return this.moderator;
            }

            set
            {
                this.moderator = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Mandatory]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string Notes
        {
            get
            {
                return this.notes;
            }

            set
            {
                this.notes = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only private comments.
        /// </summary>
        public bool? OnlyPrivateComments
        {
            get
            {
                return this.onlyPrivateComments;
            }

            set
            {
                this.onlyPrivateComments = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int? OrganizationId
        {
            get
            {
                return this.organizationId;
            }

            set
            {
                this.organizationId = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        public string Phone
        {
            get
            {
                return this.phone;
            }

            set
            {
                this.phone = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the photo.
        /// </summary>
        public IAttachment Photo
        {
            get
            {
                return this.photo;
            }

            set
            {
                this.photo = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether restricted agent.
        /// </summary>
        public bool? RestrictedAgent
        {
            get
            {
                return this.restrictedAgent;
            }

            set
            {
                this.restrictedAgent = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [Mandatory]
        public string Role
        {
            get
            {
                return this.role;
            }

            set
            {
                this.role = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        [ReadOnly]
        public bool? Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared agent.
        /// </summary>
        [ReadOnly]
        public bool? SharedAgent { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        public string Signature
        {
            get
            {
                return this.signature;
            }

            set
            {
                this.signature = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether suspended.
        /// </summary>
        public bool? Suspended
        {
            get
            {
                return this.suspended;
            }

            set
            {
                this.suspended = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get
            {
                return this.tags != null ? this.tags.ToArray() : null;
            }

            set
            {
                this.tags = value != null ? value.ToArray() : new string[0];
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ticket restriction.
        /// </summary>
        public string TicketRestriction
        {
            get
            {
                return this.ticketRestriction;
            }

            set
            {
                this.ticketRestriction = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        public string TimeZone
        {
            get
            {
                return this.timeZone;
            }

            set
            {
                this.timeZone = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        [ReadOnly]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        [ReadOnly]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the user fields.
        /// </summary>
        public IEnumerable<CustomField> UserFields
        {
            get
            {
                return this.userFields != null ? this.userFields.ToArray() : null;
            }

            set
            {
                this.userFields = value != null ? value.ToArray() : new CustomField[0];
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether verified.
        /// </summary>
        public bool? Verified
        {
            get
            {
                return this.verified;
            }

            set
            {
                this.verified = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion
    }
}