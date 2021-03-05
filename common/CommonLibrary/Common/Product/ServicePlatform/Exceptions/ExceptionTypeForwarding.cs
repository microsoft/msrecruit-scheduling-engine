//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using ServicePlatform.Exceptions;
using ServicePlatform.Privacy;
using System.Runtime.CompilerServices;

/*
 * To prevent breaking changes as types were moved out of this assembly to allow consumption by portable code,
 * here we forward those types to the ServicePlatform.Exceptions assembly.
 */
//[assembly: TypeForwardedTo(typeof(MonitoredExceptionMetadataAttribute))]
//[assembly: TypeForwardedTo(typeof(ExceptionCustomDataAttribute))]
//[assembly: TypeForwardedTo(typeof(CustomData))]
//[assembly: TypeForwardedTo(typeof(ErrorCodes))]
//[assembly: TypeForwardedTo(typeof(MonitoredExceptionKind))]
//[assembly: TypeForwardedTo(typeof(ServiceError))]
//[assembly: TypeForwardedTo(typeof(PrivacyLevel))]
//[assembly: TypeForwardedTo(typeof(PrivateInformation))]
//[assembly: TypeForwardedTo(typeof(ServiceErrorJsonSerializer))]

//// Ignore obsolete warning
//#pragma warning disable 618
//[assembly: TypeForwardedTo(typeof(IContainsPrivateInformation))]
#pragma warning restore 618
