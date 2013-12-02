
#region Copyright Notice
/*
 * Digi-Data Corporation Vault Explorer
 *
 * Written by Jay Miller.  Copyright 2008 Digi-Data Corporation.  All rights
 * reserved.
 * 
 * Title to these Materials and all copies thereof remain with Digi-Data
 * Corporation or its suppliers.  The Materials are copyrighted and are
 * protected by United States copyright laws and international treaty
 * provisions.  Unauthorized copying or the removal of any product
 * identification, copyright notice, or other notice from the Materials is
 * expressly prohibited.  Unauthorized copying of the Materials is expressly
 * prohibited.  Digi-Data Corporation does not grant any express or implied
 * right to you under Digi-Data Corporation patents, copyrights, trademarks,
 * or trade secret information.
 */
#endregion

using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace DigiData.SVN.JiraPrecommit
{
    /// <summary>
    /// A base class for exceptions thrown by the Atlassian SDK.
    /// </summary>
    [Serializable]
    public class AtlassianException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtlassianException"/> class.
        /// </summary>
        public AtlassianException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlassianException"/>
        /// class with the specified error message.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        public AtlassianException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlassianException"/>
        /// class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.  If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public AtlassianException(string message, Exception innerException) :
            base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlassianException"/>
        /// class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected AtlassianException(SerializationInfo info,
              StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initializes a new <see cref="AtlassianException"/>
        /// based on the raw SDK exception.
        /// </summary>
        /// <param name="e">The exception thrown by the SDK.</param>
        public static AtlassianException Create(FaultException e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            var m = Regex.Match(e.Message, @"com\.atlassian\.jira\.rpc\.exception\.(?<exception>\w+)Exception: (?<message>.*)");
            if (!m.Success)
                throw new AtlassianException(e.Message);
            var message = m.Groups["message"].Value;

            switch (m.Groups["exception"].Value)
            {
                case "RemoteAuthentication": return new AuthenticationException(message);
                case "RemoteValidation": return new IssueNotFoundException(message);
                default: return new AtlassianException(message);
            }
        }
    }
}
