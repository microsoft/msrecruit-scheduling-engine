//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------

//---------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//---------------------------------------------------------------------------
/* tslint:disable:no-trailing-whitespace */
        
export enum AccessType {
    ExportData = 0,
}

export interface Activity {
    stage?: JobStage;
    activityType?: JobApplicationActivityType;
    location?: string;
    description?: string;
    plannedStartTime?: Date;
    plannedEndTime?: Date;
    status?: JobApplicationActivityStatus;
    statusReason?: JobApplicationActivityStatusReason;
}

export enum ActivityAudience {
    HiringTeam = 0,
    InternalCandidates = 1,
    ExternalCandidates = 2,
    AllCandidates = 3,
}

export interface AddCandidateProjectResponse {
    linkUrl?: string;
}

export interface Address {
    AddressLine1?: string;
    AddressLine2?: string;
    City?: string;
    State?: string;
    Country?: CountryCode;
    PostalCode?: string;
}

export interface Applicant extends Person {
    id?: string;
    guid?: string;
    b2cObjectId?: string;
    status?: CandidateStatus;
    statusReason?: CandidateStatusReason;
    externalId?: string;
    externalSource?: CandidateExternalSource;
    preferredPhone?: CandidatePreferredPhone;
    internal?: boolean;
    gender?: CandidateGender;
    ethnicOrigin?: CandidateEthnicOrigin;
    attachments?: ApplicantAttachment[];
    alertOnApplicationChange?: boolean;
    trackings?: ApplicantTagTracking[];
    jobs?: Job[];
    extendedAttributes?: { [key: string]: string; };
    isProspect?: boolean;
    workExperience?: WorkExperience[];
    education?: Education[];
    skillSet?: string[];
    socialIdentities?: SocialIdentity[];
    rank?: Rank;
    candidateTalentSource?: TalentSource;
}

export interface ApplicantAttachment {
    id?: string;
    documentType?: CandidateAttachmentDocumentType;
    type?: CandidateAttachmentType;
    name?: string;
    description?: string;
    reference?: string;
    userAction?: UserAction;
    isJobApplicationAttachment?: boolean;
    uploadedBy?: Person;
    uploadedDateTime?: Date;
}

export interface ApplicantProfile extends Person {
    id?: string;
    guid?: string;
    b2cObjectId?: string;
    isInternal?: boolean;
    status?: CandidateStatus;
    statusReason?: CandidateStatusReason;
    externalSource?: CandidateExternalSource;
    preferredPhone?: CandidatePreferredPhone;
    gender?: CandidateGender;
    ethnicOrigin?: CandidateEthnicOrigin;
    attachments?: ApplicantAttachment[];
    alertOnApplicationChange?: boolean;
    extendedAttributes?: { [key: string]: string; };
    workExperience?: WorkExperience[];
    education?: Education[];
    skillSet?: string[];
    talentSource?: TalentSource;
}

export interface ApplicantProfileWithApplicationDetails {
    identityProvider: string;
    identityProviderUsername: string;
    isTermsAcceptedByCandidate: boolean;
    applicantProfile: ApplicantProfile;
}

export interface ApplicantProfileWithLoginDetails {
    identityProvider: string;
    identityProviderUsername: string;
    applicantProfile: ApplicantProfile;
}

export enum Rank {
    Silver = 0,
}

export interface ApplicantsMetadata {
    IVApplicants?: IVApplicant[];
    Total?: number;
}

export interface ApplicantsMetadataRequest {
    Skip?: number;
    Take?: number;
    SearchText?: string;
    Stage?: JobApplicationActivityType;
}

export interface ApplicantTag {
    id?: string;
    name?: string;
}

export interface ApplicantTagTracking {
    id?: string;
    owner?: Person;
    category?: CandidateTrackingCategory;
    tags?: ApplicantTag[];
}

export interface Application extends TalentBaseContract {
    id?: string;
    candidate?: Applicant;
    hiringTeam?: HiringTeamMember[];
    externalStatus?: string;
    status?: JobApplicationStatus;
    statusReason?: JobApplicationStatusReason;
    rejectionReason?: OptionSetValue;
    currentStage?: JobStage;
    currentApplicationStage?: ApplicationStage;
    currentStageStatus?: JobApplicationStageStatus;
    currentStageStatusReason?: JobApplicationStageStatusReason;
    assessmentStatus?: AssessmentStatus;
    invitationId?: string;
    applicationDate?: Date;
    comment?: string;
    schedules?: StageScheduleEvent[];
    externalSource?: JobApplicationExternalSource;
    externalId?: string;
    notes?: ApplicationNote[];
    stages?: ApplicationStage[];
    isProspect?: boolean;
    jobOpeningPosition?: JobOpeningPosition;
    extendedAttributes?: { [key: string]: string; };
    userPermissions?: ApplicationPermission[];
    applicationTalentSource?: TalentSource;
}

export interface ApplicationConfiguration {
    sendMailToCandidate?: boolean;
    enforceApplicantForResume?: boolean;
}

export interface ApplicationNote extends TalentBaseContract {
    id?: string;
    text?: string;
    visibility?: CandidateNoteVisibility;
    ownerObjectId?: string;
    ownerName?: string;
    ownerEmail?: string;
    createdDate?: Date;
    noteType?: CandidateNoteType;
}

export enum ApplicationNoteType {
    Application = 0,
    Offer = 1,
}

export enum ApplicationPermission {
    ReadNote = 0,
    CreateNote = 1,
    CreateOffer = 2,
    ReadApplication = 3,
    UpdateApplication = 4,
    DeleteApplication = 5,
    RejectApplicant = 6,
    ViewAllActivities = 7,
    UpdateAllActivities = 8,
}

export interface ApplicationSchedule {
    id?: string;
    stage?: JobStage;
    scheduleEventId?: string;
    scheduleState?: JobApplicationScheduleState;
    application?: Application;
    stageOrder?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
    scheduleAvailabilities?: ScheduleAvailability[];
}

export interface ApplicationStage extends TalentBaseContract {
    stage: JobStage;
    order: number;
    displayName?: string;
    description?: string;
    stageActivities?: StageActivity[];
    isActiveStage?: boolean;
    totalActivities?: number;
    completedActivities?: number;
    lastCompletedActivityDateTime?: Date;
}

export enum AppPermission {
    ReadTalentPool = 0,
    CreateTalentPool = 1,
    ReadTemplate = 2,
    CreateTemplate = 3,
    CreateJob = 4,
    ReadEmailTemplate = 5,
    CreateEmailTemplate = 6,
    AccessAdminCenter = 7,
    ViewAllJobs = 8,
    ViewAllJobApplications = 9,
    ViewAllTalentPools = 10,
    ViewAllTemplates = 11,
    UpdateAllCandidates = 12,
    ViewAnalytics = 13,
}

export interface AppPermissions {
    userPermissions?: AppPermission[];
}

export interface AppRoleDeleteRequestPayload {
    userObjectIds: string[];
}

export interface AppRoles {
    appRoles: TalentApplicationRole[];
}

export interface AppRoleUpsertRequest {
    userObjectId?: string;
    user?: Person;
    userRoles: TalentApplicationRole[];
}

export interface AppRoleUpsertRequestPayload {
    roleUpsertRequests: AppRoleUpsertRequest[];
}

export enum ArtifactType {
    PDF = 0,
    DOC = 1,
    JPG = 2,
    DOCX = 3,
    AVI = 4,
    MP4 = 5,
}

export interface AssessmentConfiguration {
    jobAssessments?: JobAssessment[];
}

export interface AssessmentConnectionsRequest {
    jobOpeningId?: string;
    providers?: AssessmentProvider[];
}

export interface AssessmentInstructions {
    Introduction?: string;
    Ending?: string;
}

export interface AssessmentProject {
    id?: string;
    created?: string;
    title?: string;
    previewURL?: string;
    projectStatus?: string;
    instructions?: AssessmentInstructions;
    templates?: ExternalAssessment[];
    users?: AssessmentUser[];
    companyName?: string;
    jobTitle?: string;
    jobDescription?: string;
}

export interface AssessmentProjectContributors {
    projectId?: string;
    addContributors?: AssessmentUser[];
    removeContributors?: AssessmentUser[];
}

export enum AssessmentProvider {
    Default = 0,
    Koru = 1,
}

export interface AssessmentQuestion {
    questionType?: number;
}

export enum AssessmentReportType {
    Unspecified = 0,
    BackgroundCheck = 1,
    Skill = 2,
    Personality = 3,
}

export enum AssessmentSensitivityType {
    High = 0,
    Medium = 1,
    Low = 2,
}

export enum AssessmentStatus {
    NotStarted = 0,
    Started = 1,
    NeedGrading = 2,
    Completed = 3,
}

export interface AssessmentUser {
    userId?: string;
    role?: number;
    name?: string;
}

export interface AttachmentDescription {
    fileSize: number;
    fileUploaderId: string;
}

export interface Attendee {
    user: GraphPerson;
    responseStatus?: InvitationResponseStatus;
    isResponseStatusInvalid?: boolean;
    responseComment?: string;
    responseDateTime?: Date;
    freeBusyStatus?: FreeBusyScheduleStatus;
}

export interface AudioInfo {
    tollNumber: string;
    tollFreeNumber: string;
    conferenceId: string;
    dialInUrl: string;
}

export interface BrandingImage {
    uri: string;
    type: BrandingImageType;
}

export enum BrandingImageType {
    Logo = 0,
}

export interface BroadbeanClientCredentials {
    username: string;
    clientId: string;
    encryptionToken: string;
}

export interface BulkUploadResponse {
    version?: number;
}

export interface CalendarBody {
    contentType: string;
    content: string;
}

export interface SingleValueLegacyExtendedProperty {
    id?: string;
    value?: string;
}

export interface CandidacyDetails {
    CandidacyId?: string;
    RequisitionId?: string;
    JobTitle?: string;
    Candidate?: CandidateInformation;
    HiringManager?: string;
    Recruiter?: string;
    CandidacyStage?: CandidacyStage;
    CandidacyStatus?: string;
    CandidateScore?: string;
}

export interface CandidacySearchRequest {
    RequisitionFilter?: string[];
    StageFilter?: CandidacyStage;
    SearchText?: string;
    Skip?: number;
    Take?: number;
}

export enum CandidacyStage {
    Incomplete = 0,
    Apply = 1,
    Screen = 2,
    Interview = 3,
    Offer = 4,
    PreOnboard = 5,
    Dispositioned = 6,
}

export enum CandidateAttachmentDocumentType {
    PDF = 0,
    DOC = 1,
    JPG = 2,
    DOCX = 3,
    AVI = 4,
    MP4 = 5,
    HTML = 6,
    TXT = 7,
    ODT = 8,
    RTF = 9,
    PPTX = 10,
}

export enum CandidateAttachmentType {
    NotSpecified = 0,
    Resume = 1,
    CoverLetter = 2,
    Portfolio = 3,
    Video = 4,
}

export enum CandidateDisabilityStatus {
    NotSpecified = 0,
    Yes = 1,
    No = 2,
    DoNotWantToAnswer = 3,
}

export enum CandidateEthnicOrigin {
    NotSpecified = 0,
    AfricanAmericanOrBlack = 1,
    AmericanIndianOrAlaskaNative = 2,
    Asian = 3,
    CaucasianOrWhile = 4,
    HispanicOrLatino = 5,
    MultiRacial = 6,
    NativeHawaiianOrOtherPacificIslander = 7,
    DoNotWantToAnswer = 8,
}

export enum CandidateExternalSource {
    Internal = 0,
    MSDataMall = 1,
    LinkedIn = 2,
    Greenhouse = 3,
    ICIMS = 4,
}

export enum CandidateGender {
    NotSpecified = 0,
    Male = 1,
    Female = 2,
    DoNotWantToAnswer = 3,
}

export interface CandidateInformation {
    CandidateID?: string;
    ExternalCandidateID?: string;
    OID: string;
    FullName: string;
    GivenName: string;
    MiddleName: string;
    Surname: string;
    EmailPrimary: string;
    PhonePrimary: string;
    AttachmentID: string;
    AttachmentName: string;
    Educations: CandidateEducationInformation[];
    WorkExperiences: CandidateWorkExperienceInformation[];
    Skills: CandidateSkillInformation[];
}

export interface CandidateEducationInformation {
    School: string;
    Degree: string;
    FieldOfStudy: string;
    Grade: string;
    ActivitiesSocieties: string;
    Description: string;
    FromMonthYear: Date;
    ToMonthYear: Date;
}

export interface CandidateWorkExperienceInformation {
    Title: string;
    Organization: string;
    Location: string;
    Description: string;
    IsCurrentPosition: boolean;
    FromMonthYear: Date;
    ToMonthYear: Date;
}

export interface CandidateSkillInformation {
    Skill: string;
}

export enum CandidateMilitaryStatus {
    NotSpecified = 0,
    ServingOrServed = 1,
    NotServed = 2,
    DoNotWantToAnswer = 3,
}

export enum CandidateNoteType {
    Application = 0,
    Offer = 1,
}

export enum CandidateNoteVisibility {
    Private = 0,
    Public = 1,
}

export interface CandidatePersonalDetails extends TalentBaseContract {
    Gender?: CandidateGender;
    Ethnicity?: CandidateEthnicOrigin;
    DisabilityStatus?: CandidateDisabilityStatus;
    VeteranStatus?: CandidateVeteranStatus;
    MilitaryStatus?: CandidateMilitaryStatus;
    submittedOn?: Date;
    jobApplicationActivityId?: string;
}

export enum CandidatePreferredPhone {
    Home = 0,
    Work = 1,
    Mobile = 2,
}

export interface CandidateRecommendationFeedback {
    applicantId?: string;
    interested?: boolean;
    experience?: boolean;
    skills?: boolean;
    education?: boolean;
    other?: boolean;
}

export enum CandidateReferenceSource {
    NotSpecified = 0,
    SuggestedByCandidate = 1,
    FirstConnection = 2,
    SecondConnection = 3,
    Other = 4,
}

export interface CandidateScheduleCommunication {
    scheduleId: string;
    isInterviewScheduleShared: boolean;
    isInterviewerNameShared: boolean;
}

export interface CandidateSearchRequest {
    skip?: number;
    take?: number;
    facetSearchRequest?: FacetSearchRequest[];
}

export interface CandidateSearchResponse {
    candidates?: Applicant[];
    filters?: FacetResponse[];
    total?: number;
}

export enum CandidateSocialNetworkConversationContentType {
    Mail = 0,
    Note = 1,
}

export enum CandidateStatus {
    Available = 0,
    NotAvailable = 1,
}

export enum CandidateStatusReason {
    Available = 0,
    HappyInPosition = 1,
    Blacklisted = 2,
    CandidateNotInterested = 3,
}

export enum CandidateTrackingCategory {
    FutureInterest = 0,
}

export enum CandidateVeteranStatus {
    NotSpecified = 0,
    ProtectedVeteran = 1,
    NotProtectedVeteran = 2,
    DoNotWantToAnswer = 3,
}

export interface ConferenceInfo {
    id: string;
    subject: string;
    joinUrl: string;
    audio: AudioInfo;
    admitUsers: string;
    joinInfo: string;
    provider: ConferenceProvider;
}

export enum ConferenceProvider {
    MicrosoftTeams = 0,
    Skype = 1,
}

export interface ConferenceRequest {
    startTime: Date;
    endTime: Date;
    subject: string;
    participantEmailAddresses: string[];
}

export enum CountryCode {
    AF = 0,
    AX = 1,
    AL = 2,
    DZ = 3,
    AS = 4,
    AD = 5,
    AO = 6,
    AI = 7,
    AQ = 8,
    AG = 9,
    AR = 10,
    AM = 11,
    AW = 12,
    AU = 13,
    AT = 14,
    AZ = 15,
    BS = 16,
    BH = 17,
    BD = 18,
    BB = 19,
    BY = 20,
    BE = 21,
    BZ = 22,
    BJ = 23,
    BM = 24,
    BT = 25,
    BO = 26,
    BQ = 27,
    BA = 28,
    BW = 29,
    BV = 30,
    BR = 31,
    IO = 32,
    VG = 33,
    BN = 34,
    BG = 35,
    BF = 36,
    BI = 37,
    CV = 38,
    KH = 39,
    CM = 40,
    CA = 41,
    KY = 42,
    CF = 43,
    TD = 44,
    CL = 45,
    CN = 46,
    CX = 47,
    CC = 48,
    CO = 49,
    KM = 50,
    CG = 51,
    CD = 52,
    CK = 53,
    CR = 54,
    CI = 55,
    HR = 56,
    CU = 57,
    CW = 58,
    CY = 59,
    CZ = 60,
    DK = 61,
    DJ = 62,
    DM = 63,
    DO = 64,
    EC = 65,
    EG = 66,
    SV = 67,
    GQ = 68,
    ER = 69,
    EE = 70,
    ET = 71,
    FK = 72,
    FO = 73,
    FJ = 74,
    FI = 75,
    FR = 76,
    GF = 77,
    PF = 78,
    TF = 79,
    GA = 80,
    GM = 81,
    GE = 82,
    DE = 83,
    GH = 84,
    GI = 85,
    GR = 86,
    GL = 87,
    GD = 88,
    GP = 89,
    GU = 90,
    GT = 91,
    GG = 92,
    GN = 93,
    GW = 94,
    GY = 95,
    HT = 96,
    HM = 97,
    HN = 98,
    HK = 99,
    HU = 100,
    IS = 101,
    IN = 102,
    ID = 103,
    IR = 104,
    IQ = 105,
    IE = 106,
    IM = 107,
    IL = 108,
    IT = 109,
    JM = 110,
    XJ = 111,
    JP = 112,
    JE = 113,
    JO = 114,
    KZ = 115,
    KE = 116,
    KI = 117,
    KR = 118,
    XK = 119,
    KW = 120,
    KG = 121,
    LA = 122,
    LV = 123,
    LB = 124,
    LS = 125,
    LR = 126,
    LY = 127,
    LI = 128,
    LT = 129,
    LU = 130,
    MO = 131,
    MK = 132,
    MG = 133,
    MW = 134,
    MY = 135,
    MV = 136,
    ML = 137,
    MT = 138,
    MH = 139,
    MQ = 140,
    MR = 141,
    MU = 142,
    YT = 143,
    MX = 144,
    FM = 145,
    MD = 146,
    MC = 147,
    MN = 148,
    ME = 149,
    MS = 150,
    MA = 151,
    MZ = 152,
    MM = 153,
    NA = 154,
    NR = 155,
    NP = 156,
    NL = 157,
    NC = 158,
    NZ = 159,
    NI = 160,
    NE = 161,
    NG = 162,
    NU = 163,
    NF = 164,
    KP = 165,
    MP = 166,
    NO = 167,
    OM = 168,
    PK = 169,
    PW = 170,
    PS = 171,
    PA = 172,
    PG = 173,
    PY = 174,
    PE = 175,
    PH = 176,
    PN = 177,
    PL = 178,
    PT = 179,
    PR = 180,
    QA = 181,
    RE = 182,
    RO = 183,
    RU = 184,
    RW = 185,
    XS = 186,
    BL = 187,
    KN = 188,
    LC = 189,
    MF = 190,
    PM = 191,
    VC = 192,
    WS = 193,
    SM = 194,
    ST = 195,
    SA = 196,
    SN = 197,
    RS = 198,
    SC = 199,
    SL = 200,
    SG = 201,
    XE = 202,
    SX = 203,
    SK = 204,
    SI = 205,
    SB = 206,
    SO = 207,
    ZA = 208,
    GS = 209,
    SS = 210,
    ES = 211,
    LK = 212,
    SH = 213,
    SD = 214,
    SR = 215,
    SJ = 216,
    SZ = 217,
    SE = 218,
    CH = 219,
    SY = 220,
    TW = 221,
    TJ = 222,
    TZ = 223,
    TH = 224,
    TL = 225,
    TG = 226,
    TK = 227,
    TO = 228,
    TT = 229,
    TN = 230,
    TR = 231,
    TM = 232,
    TC = 233,
    TV = 234,
    UM = 235,
    VI = 236,
    UG = 237,
    UA = 238,
    AE = 239,
    GB = 240,
    US = 241,
    UY = 242,
    UZ = 243,
    VU = 244,
    VA = 245,
    VE = 246,
    VN = 247,
    WF = 248,
    YE = 249,
    ZM = 250,
    ZW = 251,
}

export interface CountryMetadata {
    countryCode: string;
    country: string;
    state: string;
    city: string;
}

export enum CurrencyCode {
    USD = 0,
    EUR = 1,
    JPY = 2,
    GBP = 3,
    AUD = 4,
    CAD = 5,
    CHF = 6,
    SEK = 7,
    MXN = 8,
    AED = 9,
    AFN = 10,
    ALL = 11,
    AMD = 12,
    ANG = 13,
    AOA = 14,
    ARS = 15,
    AWG = 16,
    AZN = 17,
    BAM = 18,
    BBD = 19,
    BDT = 20,
    BGN = 21,
    BHD = 22,
    BIF = 23,
    BMD = 24,
    BND = 25,
    BOB = 26,
    BOV = 27,
    BRL = 28,
    BSD = 29,
    BTN = 30,
    BWP = 31,
    BYN = 32,
    BYR = 33,
    BZD = 34,
    CDF = 35,
    CHE = 36,
    CHW = 37,
    CLF = 38,
    CLP = 39,
    CNY = 40,
    COP = 41,
    COU = 42,
    CRC = 43,
    CUC = 44,
    CUP = 45,
    CVE = 46,
    CZK = 47,
    DJF = 48,
    DKK = 49,
    DOP = 50,
    DZD = 51,
    EGP = 52,
    ERN = 53,
    ETB = 54,
    FJD = 55,
    FKP = 56,
    GEL = 57,
    GHS = 58,
    GIP = 59,
    GMD = 60,
    GNF = 61,
    GTQ = 62,
    GYD = 63,
    HKD = 64,
    HNL = 65,
    HRK = 66,
    HTG = 67,
    HUF = 68,
    IDR = 69,
    ILS = 70,
    INR = 71,
    IQD = 72,
    IRR = 73,
    ISK = 74,
    JMD = 75,
    JOD = 76,
    KES = 77,
    KGS = 78,
    KHR = 79,
    KMF = 80,
    KPW = 81,
    KRW = 82,
    KWD = 83,
    KYD = 84,
    KZT = 85,
    LAK = 86,
    LBP = 87,
    LKR = 88,
    LRD = 89,
    LSL = 90,
    LYD = 91,
    MAD = 92,
    MDL = 93,
    MGA = 94,
    MKD = 95,
    MMK = 96,
    MNT = 97,
    MOP = 98,
    MRO = 99,
    MUR = 100,
    MVR = 101,
    MWK = 102,
    MXV = 103,
    MYR = 104,
    MZN = 105,
    NAD = 106,
    NGN = 107,
    NIO = 108,
    NOK = 109,
    NPR = 110,
    NZD = 111,
    OMR = 112,
    PAB = 113,
    PEN = 114,
    PGK = 115,
    PHP = 116,
    PKR = 117,
    PLN = 118,
    PYG = 119,
    QAR = 120,
    RON = 121,
    RSD = 122,
    RUB = 123,
    RWF = 124,
    SAR = 125,
    SBD = 126,
    SCR = 127,
    SDG = 128,
    SGD = 129,
    SHP = 130,
    SLL = 131,
    SOS = 132,
    SRD = 133,
    SSP = 134,
    STD = 135,
    SVC = 136,
    SYP = 137,
    SZL = 138,
    THB = 139,
    TJS = 140,
    TMT = 141,
    TND = 142,
    TOP = 143,
    TRY = 144,
    TTD = 145,
    TWD = 146,
    TZS = 147,
    UAH = 148,
    UGX = 149,
    USN = 150,
    UYI = 151,
    UYU = 152,
    UZS = 153,
    VEF = 154,
    VND = 155,
    VUV = 156,
    WST = 157,
    XAF = 158,
    XAG = 159,
    XAU = 160,
    XBA = 161,
    XBB = 162,
    XBC = 163,
    XBD = 164,
    XCD = 165,
    XDR = 166,
    XOF = 167,
    XPD = 168,
    XPF = 169,
    XPT = 170,
    XSU = 171,
    XTS = 172,
    XUA = 173,
    XXX = 174,
    YER = 175,
    ZAR = 176,
    ZMW = 177,
    ZWL = 178,
}

export interface DashboardActivitiesRequest {
    startTime?: Date;
    endTime?: Date;
    skip?: number;
    take?: number;
    continuationToken?: string;
}

export interface DashboardActivitiesResponse {
    dashboardActivities?: DashboardActivity[];
    continuationToken?: string;
}

export interface DashboardActivity {
    startTime?: Date;
    endTime?: Date;
    jobApplicationActivityType?: JobApplicationActivityType;
    activity?: StageActivity;
    participants?: Person[];
    stageOrder?: number;
    applicant?: Applicant;
    job?: Job;
    jobApplicationId?: string;
    jobApplicationStage?: ApplicationStage;
    attendees?: ScheduleAttendee[];
    isDelegated?: boolean;
}

export enum DashboardFilterBy {
    Active = 0,
    Draft = 1,
    Archived = 2,
}

export enum DashboardSortedBy {
    Name = 0,
    ModifiedDate = 1,
}

export interface Delegate extends Person {
    onBehalfOfUserObjectId?: string;
}

export interface DelegateUpsertRequestPayload {
    delegates: Delegate[];
}

export interface DelegationQueryParameters extends PaginationAndSortQueryParameters {
    includeWorker?: boolean;
    searchToEmail?: string;
    searchFromEmail?: string;
}

export interface DelegationRequest extends TalentBaseContract {
    DelegationId?: string;
    DelegationStatus?: DelegationStatus;
    DelegationStatusReason?: string;
    RequestStatus?: RequestStatus;
    RequestStatusReason?: string;
    FromDate: Date;
    ToDate: Date;
    UTCOffsetInMinutes: string;
    From: Person;
    To: Person;
    RequestedBy: Person;
    Notes?: string;
}

export interface DelegationRequestPagedResponse {
    items?: DelegationRequest[];
    nextPage?: string;
    previousPage?: string;
    pageNumber?: number;
    pageSize?: number;
    totalCount?: number;
}

export enum DelegationStatus {
    Active = 0,
    Inactive = 1,
}

export interface Department {
    id?: string;
    description?: string;
    name?: string;
    parentDepartment?: Department;
    jobPositions?: JobOpeningPosition[];
}

export interface Education {
    EducationId?: string;
    school?: string;
    degree?: string;
    fieldOfStudy?: string;
    grade?: string;
    description?: string;
    FromMonthYear?: Date;
    ToMonthYear?: Date;
}

export interface EmailContent {
    content?: string;
    order?: number;
    emailContentType?: EmailContentType;
}

export enum EmailContentType {
    Subject = 0,
    Header = 1,
    EmailBody = 2,
    Closing = 3,
    Footer = 4,
}

export interface EmailNotificationRequest {
    jobApplicationId: string;
    emailBody: string;
    emailFooter?: string;
    subject: string;
    mailTo: GraphPerson[];
    mailCC?: GraphPerson[];
}

export interface EmailTemplate {
    id?: string;
    templateName: string;
    appName: string;
    templateType: string;
    isGlobal?: boolean;
    to?: string[];
    cc?: string[];
    additionalCc?: string[];
    bcc?: string[];
    subject?: string;
    header?: string;
    body?: string;
    attachments?: FileAttachmentRequest;
    closing?: string;
    footer?: string;
    isDefault?: boolean;
    isAutosent?: boolean;
    creator?: string;
    language?: string;
}

export interface EmailTemplateLegacy {
    id?: string;
    tenantId?: string;
    userObjectId?: string;
    templateName?: string;
    templateType?: TemplateType;
    isDefault?: boolean;
    isAutosent?: boolean;
    isTentative?: boolean;
    ccEmailAddressRoles?: JobParticipantRole[];
    ccEmailAddressList?: string[];
    bccEmailAddressRoles?: JobParticipantRole[];
    bccEmailAddressList?: string[];
    primaryEmailRecipients?: GraphPerson[];
    emailContent?: EmailContent[];
    emailTokenList?: EmailTemplateTokens[];
    fromAddressMode?: SendEmailFromAddressMode;
}

export interface EmailTemplateSettings {
    emailTemplateFooterText?: string;
    emailTemplatePrivacyPolicyLink?: string;
    emailTemplateTermsAndConditionsLink?: string;
    shouldDisableEmailEdits?: boolean;
    emailTemplateHeaderImgUrl?: string;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}

export enum EmailTemplateTokens {
    Job_Title = 0,
    Candidate_Name = 1,
    Candidate_FirstName = 2,
    Company_Name = 3,
    Requester_Name = 4,
    Requester_Email = 5,
    Requester_Role = 6,
    HiringManager_Name = 7,
    HiringManager_Email = 8,
    Interview_Date = 9,
    Recruiter_Name = 10,
    Recruiter_Email = 11,
    Interview_Details_Table = 12,
    Call_To_Action_Link = 13,
    First_Interviewer_Name = 14,
    First_Interview_Time = 15,
    First_Interviewer_Job_Title = 16,
    Scheduler_Email = 17,
    Scheduler_Phone_Number = 18,
    Skype_Link = 19,
    Job_Id = 20,
    External_Job_Id = 21,
    Interviewer_FirstName = 22,
    Scheduler_First_Name = 23,
    ResponseStatus = 24,
    OfferExpiryDate = 25,
    OfferPackageURL = 26,
    Offer_Approver_Name = 27,
    Offer_Approver_Email = 28,
    Offer_Rejecter_Name = 29,
    Offer_Rejecter_Email = 30,
    Identity_Provider = 31,
    Identity_Provider_User_Name = 32,
    Job_Approver_Name = 33,
    Job_Approver_Email = 34,
    Job_Requester_Name = 35,
    Job_Requester_Email = 36,
    Requester_Signature_Name = 37,
}

export enum EntityType {
    JobOpeningPosition = 0,
    JobPost = 1,
    Candidate = 2,
    JobOpening = 3,
    JobApplication = 4,
}

export interface EnvironmentDocument extends Document {
    environmentStatus: EnvironmentStatus;
    environmentHistory: HistoryEvent[];
    createdDate: Date;
}

export interface EnvironmentMap {
    id?: string;
    autoNumber?: string;
    displayName?: string;
    alias?: string;
    brandingImages?: BrandingImage[];
    companyHeadquarters?: string;
    companyWebpage?: string;
    contactEmail?: string;
    phoneNumber?: string;
    environmentId?: string;
    tenantId?: string;
    privacyLink?: string;
    tosLink?: string;
    termsAndConditionsLink?: string;
    termsAndConditionsText?: string;
}

export interface EnvironmentMetadata {
    id: string;
    applicationCount: number;
}

export enum EnvironmentMode {
    Cds = 0,
    Falcon = 1,
    Both = 2,
    Xrm = 3,
    XrmAndFalcon = 4,
}

export interface EnvironmentSettings {
    id: string;
    templateSetting?: TemplateSettings;
    featureSettings?: FeatureSettings[];
    integrationSettings?: { [key: string]: IntegrationSetting; };
    offerSettings?: OfferSettings;
    eSignSettings?: ESignSettings;
    emailTemplateSettings?: EmailTemplateSettings;
    isPremium?: boolean;
}

export interface EnvironmentStatus {
    bapLocation: string;
    clusterUri: string;
    endDate: Date;
    environmentStatusCode: EnvironmentStatusCode;
    id: string;
    isTenantTakenOver: boolean;
    isRelSearchEnabledOnFirstLogin?: boolean;
    namespaceId: string;
    namespaceRuntimeUri: string;
    packageStatuses: PackageStatus[];
    previousTenantId: string;
    startDate: Date;
    tenantId: string;
    testDrive: boolean;
    errorDetails: string;
    deletionDetails: string;
    environmentMode: EnvironmentMode;
    falconDatabaseId?: string;
    falconResourceName?: string;
    expirationDate?: Date;
    linkedXRMEnvironmentInformation?: LinkedXRMEnvironmentInformation;
}

export enum EnvironmentStatusCode {
    Unknown = 0,
    Installing = 1,
    Completed = 2,
    Upgrading = 3,
    Error = 4,
    NotStarted = 5,
    Deleting = 6,
    SoftDeleted = 7,
    Deleted = 8,
    Migrating = 9,
}

export interface ESignAccount {
    id?: string;
    eSignType?: ESignType;
    secret?: string;
    integrationKey?: string;
}

export interface ESignSettings {
    enabledESignType?: ESignType;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}

export enum ESignType {
    ESign = 0,
    DocuSign = 1,
    AdobeSign = 2,
}

export interface ESignUserSetup {
    authcode?: string;
    redirectUri?: string;
    apiAccessPoint?: string;
    webAccessPoint?: string;
}

export interface ESignUserStatus {
    isEsignEnabled?: boolean;
    integrationKey?: string;
    emailAddress?: string;
}

export interface ExpressionSuggestion {
    id: string;
    improperExpression?: string;
    suggestedExpression?: string;
}

export interface ExpressionSuggestionCollection {
    id: string;
    name?: string;
    reason?: string;
    suggestion?: string;
    languageCode?: string;
    isEnabled?: boolean;
    hasDefault?: boolean;
    expressionSuggestions?: ExpressionSuggestion[];
}

export interface ExpressionSuggestionStatistic {
    id: string;
    improperExpressionCount?: number;
    suggestionsTakenCount?: number;
}

export interface ExternalAssessment {
    id?: string;
    created?: string;
    title?: string;
    previewUrl?: string;
    numberOfQuestions?: number;
    instructions?: AssessmentInstructions;
    questions?: AssessmentQuestion[];
}

export interface ExternalAssessmentKoru {
    assessmentId?: string;
    assessmentType?: string;
    name?: string;
    previewUrl?: string;
    numberOfQuestions?: number;
}

export interface KoruAssessments {
    assessments?: ExternalAssessmentKoru[];
}

export interface ExternalJobPost extends TalentBaseContract {
    id?: string;
    uri?: string;
    supplier?: JobPostSupplier;
    supplierName?: string;
    isRepostAvailable?: boolean;
    userAction?: UserAction;
    extendedAttributes?: { [key: string]: string; };
}

export enum ExternalSource {
    Internal = 0,
    MSDataMall = 1,
    LinkedIn = 2,
    Greenhouse = 3,
    ICIMS = 4,
}

export interface FacetDetail {
    name?: string;
    isSelected?: boolean;
    count?: number;
    id?: string;
}

export interface FacetResponse {
    filter?: FacetType;
    facetDetails?: FacetDetail[];
}

export interface FacetSearchRequest {
    facet?: FacetType;
    searchText?: string;
    isFilter?: boolean;
}

export enum FacetType {
    None = -1,
    CandidateName = 0,
    City = 1,
    Degree = 2,
    IsInternal = 3,
    Location = 4,
    Organization = 5,
    School = 6,
    Skills = 7,
    State = 8,
    TalentPools = 9,
    Title = 10,
    FieldOfStudy = 11,
    Rank = 12,
    Source = 13,
}

export enum Feature {
    JobTemplate = 0,
    JobPosting = 1,
    Assessment = 2,
    OfferManagement = 3,
    Prospect = 4,
    SkypeInterview = 5,
    CandidateReccomendation = 6,
    JobReccomendation = 7,
    TalentCompanion = 8,
    PositionManagement = 9,
    CustomActivies = 10,
    BroadbeanIntegration = 11,
    Dashboard = 12,
    JobApproval = 13,
    Analytics = 14,
    EmailTemplate = 15,
    SearchEngineOptimization = 16,
    TermsAndConditions = 17,
    LinkedInIntegration = 18,
    EEO = 19,
    ProspectRecommendation = 20,
    RelevanceSearch = 21,
    ActivityAudience = 22,
    ApplyWithLinkedIn = 23,
    InclusiveHiring = 24,
    BroadbeanSetup = 25,
    JobCreationWizard = 26,
    TalentSourceTracking = 27,
    SilverMedalist = 28,
    ServiceAccount = 29,
}

export interface FeatureSettings {
    feature?: Feature;
    isEnabled?: boolean;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}

export interface Feedback {
    stage?: JobStage;
    stageOrder?: number;
    strengthComment?: string;
    weaknessComment?: string;
    overallComment?: string;
    Status?: JobApplicationAssessmentStatus;
    statusReason?: JobApplicationAssessmentStatusReason;
    isRecommendedToContinue: boolean;
    feedbackProvider?: Delegate;
    jobApplicationID: string;
}

export enum FeedbackAction {
    Save = 0,
    Submit = 1,
}

export interface FeedbackConfiguration {
    inheritFromHiringTeam?: boolean;
    viewFeedbackBeforeSubmitting?: boolean;
    editFeedbackAfterSubmitting?: boolean;
    feedbackReminderDelay?: FeedbackReminderDelay;
}

export enum FeedbackReminderDelay {
    None = 0,
    HalfDay = 1,
    OneDay = 2,
    TwoDays = 3,
    ThreeDays = 4,
    OneWeek = 5,
}

export interface FeedbackNotes {
    submittedByOID: string;
    submittedByName: string;
    participantOID: string;
    participantName: string;
    submittedDateTime: Date;
    notes: string;
}

export interface FeedbackParticipant extends Person {
    role: JobParticipantRole;
    feedbacks?: Feedback[];
}

export interface FeedbackSummary {
    Status?: JobApplicationAssessmentStatus;
    IsRecommendedToContinue?: boolean;
    SubmittedDateTime?: Date;
    InterviewerName?: string;
    OID?: string;
    RemindApplicable?: boolean;
    IsScheduleAvailable?: boolean;
    OverallComment?: string;
    FileAttachments?: FileAttachmentInfo[];
    SubmittedByOID?: string;
    SubmittedByName?: string;
    Notes?: string;
    NotesSubmittedByOID?: string;
    NotesSubmittedByName?: string;
    NotesSubmittedDateTime?: Date;
}

export interface FileAttachmentInfo {
    fileId: string;
    name: string;
    blobURI: string;
    fileLabel?: string;
    description?: string;
    documentType?: string;
    fileSize?: number;
}

export interface FileAttachmentRequest {
    files: File[];
    fileLabels: string[];
}

export enum FinalScheduleStatus {
    None = 0,
    Scheduled = 1,
    Draft = 2,
    Sending = 3,
    Failed = 4,
}

export enum FlightingContextType {
    TenantObjectId = 0,
    UserObjectId = 1,
    Upn = 2,
    EnvironmentId = 3,
    Generic = 4,
}

export interface FreeBusyRequest {
    userGroups: UserGroup[];
    utcStart?: Date;
    utcEnd?: Date;
    isRoom?: boolean;
}

export enum FreeBusyScheduleStatus {
    Free = 0,
    Tentative = 1,
    Busy = 2,
    Oof = 3,
    WorkingElsewhere = 4,
    Unknown = 5,
    NonWorkingHour = 6,
}

export interface GaugeAssessmentResultPayload {
    percentage: number;
    status?: GaugeAssessmentStatus;
    subject?: string;
    gradeUrl?: string;
    detailUrl?: string;
    additionalInformation?: string;
}

export enum GaugeAssessmentStatus {
    Added = 1,
    Sent = 2,
    Started = 3,
    Submitted = 4,
    Grade = 5,
    Done = 6,
}

export enum Gender {
    Male = 0,
    Female = 1,
    NotSpecified = 2,
    Nonspecific = 3,
}

export interface GraphPerson {
    name: string;
    id?: string;
    title?: string;
    email: string;
    givenName?: string;
    mobilePhone?: string;
    officeLocation?: string;
    preferredLanguage?: string;
    surname?: string;
    userPrincipalName?: string;
    InvitationResponseStatus?: InvitationResponseStatus;
    Comments?: string;
}

export interface GtaFlightingContext {
    name: string;
    value: string;
    contextType: FlightingContextType;
}

export interface HireMetadata {
    candidates?: Applicant[];
    jobs?: Job[];
    candidatesAppliedCount: number;
    candidatesScreenCount: number;
    candidatesInterviewCount: number;
    candidatesOfferCount: number;
    candidatesAssessmentCount: number;
}

export interface HiringTeamMember extends Person {
    role: JobParticipantRole;
    userAction?: UserAction;
    title?: string;
    ordinal?: number;
    activities?: Activity[];
    feedbacks?: Feedback[];
    delegates?: Delegate[];
    metadata?: JobApplicationParticipantMetadata;
    isDeleteAllowed?: boolean;
    AddedOnDate?: Date;
}

export interface HiringTeamMetadata {
    total?: number;
    hiringTeam?: HiringTeamMember[];
    delegates?: Delegate[];
}

export interface HiringTeamMetadataRequest {
    skip?: number;
    take?: number;
    searchText?: string;
}

export interface HistoryEvent {
    logs: string[];
    raid: string;
    date: Date;
}

export interface ImportResult {
    index: number;
    isSuccessful: boolean;
    exceptionCode: string;
    jobOpeningId?: string;
}

export interface IncentivePlanMetadata {
    organization: string;
    profession: string;
    discipline: string;
    standardTitle: string;
    incentivePlan: string;
    level: string;
}

export interface Integration {
    jobs?: Job[];
    syncTimeInTicks?: number;
}

export interface IntegrationSetting {
    value?: string;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}

export interface InterviewConfiguration {
    allowParticipantsOutsideOfLoop?: boolean;
}

export interface InterviewDetails {
    JobApplicationID: string;
    CandidateName?: string;
    PositionTitle?: string;
    SchedulesForDay?: InterviewSchedule[];
}

export interface Interviewer {
    PrimaryEmail?: string;
    Name?: string;
    OfficeGraphIdentifier?: string;
    Profession?: string;
    Role?: JobParticipantRole;
    InterviewerResponseStatus?: InvitationResponseStatus;
    InterviewerComments?: string;
}

export interface InterviewersNotificationRequest {
    jobApplicationId: string;
    interviewers: GraphPerson[];
}

export interface InterviewMeetingLocation {
    roomList: Room;
    room: Room;
}

export enum InterviewMode {
    None = 0,
    FaceToFace = 1,
    OnlineCall = 2,
}

export interface InterviewSchedule {
    InterviewStartDateTime?: Date;
    InterviewEndDateTime?: Date;
    Interviewers?: Interviewer[];
    InterviewMode?: InterviewMode;
    InterviewScheduleStatus?: ScheduleStatus;
}

export enum InvitationResponseStatus {
    None = 0,
    Accepted = 1,
    TentativelyAccepted = 2,
    Declined = 3,
    Pending = 4,
    Sending = 5,
    ResendRequired = 6,
}

export interface IVApplicant {
    JobApplicationId?: string;
    ExternalJobOpeningId?: string;
    ExternalJobApplicationId?: string;
    JobTitle?: string;
    Candidate?: CandidateInformation;
    HiringManager?: string;
    Recruiter?: string;
    participantRole?: JobParticipantRole;
    CurrentStage?: JobApplicationActivityType;
    JobDescription?: string;
}

export interface IVApplicationNote {
    id?: string;
    text?: string;
    ownerObjectId?: string;
    createdDate?: Date;
    ownerFullName?: string;
}

export enum IVApplicationRole {
    IVAdmin = 0,
    IVReadOnly = 1,
}

export interface IVPerson {
    objectId?: string;
    givenName?: string;
    middleName?: string;
    surname?: string;
    fullName?: string;
    email?: string;
    alternateEmail?: string;
    linkedIn?: string;
    linkedInAPI?: string;
    facebook?: string;
    twitter?: string;
    homePhone?: string;
    workPhone?: string;
    mobilePhone?: string;
    profession?: string;
}

export enum IVRequisitionPilotType {
    PrePilot = 0,
    FirstRingPilot = 1,
    SecondRingPilot = 2,
    ThirdRingPilot = 3,
    FourthRingPilot = 4,
    FifthRingPilot = 5,
    SixthRingPilot = 6,
}

export interface IVUser {
    ivperson: IVPerson;
    ivroles: IVApplicationRole[];
}

export interface IVUserProfile {
    ivperson: IVPerson;
    ivroles: IVApplicationRole[];
    firsttimelogin?: Date;
}

export interface IVWorker {
    workerId?: string;
    profession?: string;
    name?: PersonName;
    fullName?: string;
    emailPrimary?: string;
    alias?: string;
    phonePrimary?: string;
    officeGraphIdentifier?: string;
    isEmailContactAllowed?: boolean;
}

export interface Job extends TalentBaseContract {
    id?: string;
    title?: string;
    description?: string;
    numberOfOpenings?: number;
    numberOfOffers?: number;
    status?: JobOpeningStatus;
    statusReason?: JobOpeningStatusReason;
    location?: string;
    source?: JobOpeningExternalSource;
    externalId?: string;
    externalStatus?: string;
    createdDate?: Date;
    comment?: string;
    primaryPositionID?: string;
    positionURI?: string;
    applyURI?: string;
    skills?: string[];
    seniorityLevel?: string;
    seniorityLevelValue?: OptionSetValue;
    employmentType?: string;
    employmentTypeValue?: OptionSetValue;
    jobFunctions?: string[];
    companyIndustries?: string[];
    isTemplate?: boolean;
    jobTemplate?: JobTemplate;
    stages?: ApplicationStage[];
    hiringTeam?: HiringTeamMember[];
    applications?: Application[];
    externalJobPost?: ExternalJobPost[];
    templateParticipants?: JobOpeningTemplateParticipant[];
    jobOpeningVisibility?: JobOpeningVisibility;
    jobOpeningPositions?: JobOpeningPosition[];
    positionStartDate?: Date;
    positionEndDate?: Date;
    applicationStartDate?: Date;
    applicationCloseDate?: Date;
    postalAddress?: Address;
    extendedAttributes?: { [key: string]: string; };
    configuration?: string;
    approvalParticipants?: JobApprovalParticipant[];
    delegates?: Delegate[];
    userPermissions?: JobPermission[];
    priority?: string;
}

export enum JobApplicationActionType {
    JobApplicationCreated = 0,
    SendToInterviewers = 1,
    SendToCandidate = 2,
}

export enum JobApplicationActivityArtifactType {
    File = 0,
    Email = 1,
    Meeting = 2,
    Comment = 3,
    Image = 4,
    URL = 5,
    Video = 6,
}

export enum JobApplicationActivityStatus {
    Planned = 0,
    Started = 1,
    Completed = 2,
    Cancelled = 3,
    Skipped = 4,
}

export enum JobApplicationActivityStatusReason {
    ProcessCreated = 0,
    ProcessCompleted = 1,
    Started = 2,
    Completed = 3,
    PositionFilled = 4,
    PositionCancelled = 5,
}

export enum JobApplicationActivityType {
    IVRequested = 0,
    Interview = 1,
    Feedback = 2,
    Offer = 3,
}

export interface JobApplicationAssessmentReport {
    jobApplicationId: string;
    candidateGivenName: string;
    candidateSurname: string;
    externalAssessmentReportID: string;
    assessmentStatus: AssessmentStatus;
    assessmentURL: string;
    title: string;
    reportURL: string;
    results: JobApplicationAssessmentReportResult[];
    additionalInformation: string;
}

export interface JobApplicationAssessmentReportResult {
    ScoreType: string;
    ScoreValue: string;
    ResultSubject: string;
    AdditionalInformation: string;
    AdditionalResultData: string;
}

export enum JobApplicationAssessmentStatus {
    NotStarted = 0,
    InProgress = 1,
    Submitted = 2,
}

export enum JobApplicationAssessmentStatusReason {
    NotStarted = 0,
    InProgress = 1,
    Returned = 2,
    Completed = 3,
}

export enum JobApplicationCurrentActivity {
    Default = 0,
}

export interface ApplicantAssessmentReport {
    title: string;
    assessmentURL?: string;
    externalAssessmentReportID?: string;
    providerKey?: string;
    assessmentStatus?: AssessmentStatus;
    dateOrdered?: Date;
    dateCompleted?: Date;
    stageOrdinal?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
}

export interface JobApplicationInterview {
    InterviewerName: string;
    LinkedinIdentity: string;
    StartDate: Date;
    EndDate: Date;
    stageOrdinal?: number;
    activityOrdinal?: number;
    activitySubOrdinal?: number;
}

export interface JobApplicationDetails extends TalentBaseContract {
    ApplicationId: string;
    TenantId: string;
    CompanyName: string;
    PositionLocation: string;
    PositionTitle: string;
    JobDescription: string;
    JobPostLink?: string;
    DateApplied: Date;
    Status: JobApplicationStatus;
    ExternalStatus?: string;
    ExternalSource?: JobApplicationExternalSource;
    StatusReason?: JobApplicationStatusReason;
    RejectionReason?: OptionSetValue;
    CurrentJobStage?: JobStage;
    CurrentApplicationStage?: ApplicationStage;
    Interviews: JobApplicationInterview[];
    ApplicationSchedules?: ApplicationSchedule[];
    ApplicantAttachments?: ApplicantAttachment[];
    ApplicantAssessments?: ApplicantAssessmentReport[];
    ApplicationStages?: ApplicationStage[];
    candidatePersonalDetails?: CandidatePersonalDetails[];
}

export enum JobApplicationExternalSource {
    Internal = 0,
    MSDataMall = 1,
    LinkedIn = 2,
    Greenhouse = 3,
    ICIMS = 4,
}

export interface JobApplicationHistoryMetadata {
    jobApplicationStatus?: JobApplicationStatus;
    jobApplicationDate?: Date;
    hiringTeamMember?: HiringTeamMember;
    jobTitle?: string;
    jobOpeningId?: string;
    jobApplicationId?: string;
    rank?: Rank;
    talentSource?: TalentSource;
}

export interface JobApplicationMetadata {
    jobApplicationId?: string;
    externalJobOpeningId?: string;
    jobTitle?: string;
    jobDescription?: string;
    currentJobApplicationStageStatus?: JobApplicationStageStatus;
    currentJobOpeningStage?: JobStage;
    jobApplicationStatus?: JobApplicationStatus;
    candidate?: CandidateInformation;
    jobApplicationParticipants?: JobApplicationParticipant[];
    jobApplicationParticipantDetails?: IVWorker[];
    IsScheduleSentToCandidate?: boolean;
    HireType?: string;
    JobApplicationStatusReason?: JobApplicationStatusReason;
    isWobAuthenticated?: boolean;
}

export interface JobApplicationParticipant {
    OID?: string;
    Role?: JobParticipantRole;
    AddedOnDate?: Date;
}

export interface JobApplicationParticipantMetadata {
    stages: JobApplicationStage[];
}

export enum JobApplicationReferenceSource {
    NotSpecified = 0,
    LinkedIn = 1,
    Facebook = 2,
    Candidate = 3,
}

export interface JobApplicationRequest {
    applicationStatuses?: JobApplicationStatus[];
    skip?: number;
    take?: number;
    searchText?: string;
    stageOrder?: number;
    prospectOnly?: boolean;
}

export interface JobApplications {
    applications?: Application[];
    total?: number;
    hasOfferApplicant?: boolean;
}

export enum JobApplicationScheduleState {
    NotScheduled = 0,
    Purposed = 1,
    Invited = 2,
    Scheduled = 3,
}

export enum JobApplicationSource {
    Default = 0,
}

export interface JobApplicationStage {
    stage?: JobStage;
    order?: number;
    name?: string;
}

export enum JobApplicationStageStatus {
    Active = 0,
    Closed = 1,
}

export enum JobApplicationStageStatusReason {
    Open = 0,
    Complete = 1,
    DidNotPass = 2,
}

export enum JobApplicationStatus {
    Active = 0,
    Offered = 1,
    Closed = 2,
}

export enum JobApplicationStatusReason {
    New = 0,
    OfferPrepared = 1,
    OfferAccepted = 2,
    CandidateWithdrew = 3,
    PositionFilled = 4,
    PositionCancelled = 5,
    Application = 6,
    Education = 7,
    Experience = 8,
    SkillSet = 9,
    Competency = 10,
    Licensure = 11,
    Assessment = 12,
    OfferRejected = 13,
    SilverMedalist = 14,
    NicManagementExperience = 1001,
    NicJobRelatedEducation = 1002,
    NicOther = 1003,
    NicJobTechnicalFunctionalExperience = 1004,
    NicInconsistentJobHistory = 1005,
    NoRequiredQualification = 1006,
    OthersMoreQualified = 1007,
}

export interface JobApplicationStatusReasonPayload {
    StatusReason?: JobApplicationStatusReason;
    RejectionReason?: OptionSetValue;
    Comment?: string;
}

export interface JobApprovalParticipant extends Person {
    comment?: string;
    jobApprovalStatus?: JobApprovalStatus;
    userAction?: UserAction;
}

export interface JobApprovalPayload {
    jobApprovalStatus?: JobApprovalStatus;
    comment?: string;
}

export interface JobApprovalProcess {
    jobApprovalProcessType?: JobApprovalProcessType;
}

export enum JobApprovalProcessType {
    None = 0,
    Default = 1,
}

export enum JobApprovalStatus {
    NotStarted = 0,
    Approved = 1,
    Rejected = 2,
}

export interface JobAssessment {
    jobAssessmentID?: string;
    packageID?: string;
    jobOpeningID?: string;
    provider?: AssessmentProvider;
    providerKey?: string;
    title?: string;
    numberOfQuestions?: number;
    previewURL?: string;
    assessment?: ExternalAssessment;
    isRequired?: JobOpeningAssessmentRequirementStatus;
    stage?: JobStage;
}

export enum JobAssessmentTopicRatingType {
    YesNo = 0,
    FiveStar = 1,
}

export interface JobClosing {
    jobOpeningStatus: JobOpeningStatus;
    jobOpeningStatusReason: JobOpeningStatusReason;
    jobOpeningExternalStatus?: string;
    comment?: string;
    offerAcceptedJobApplicationIds?: string[];
}

export interface JobConfiguration {
    jobApprovalProcess?: JobApprovalProcess;
}

export interface JobMatch {
    jobOpeningId: string;
    externalJobOpeningId: string;
    description?: string;
    jobLocation?: Address;
    jobTitle?: string;
    jobOpeningProperties?: JobOpeningProperties;
    computeResult?: JobSkill[];
    score: number;
}

export interface JobMetadata {
    jobs?: Job[];
    total?: number;
    continuationToken?: string;
}

export interface JobMetadataRequest {
    jobStatuses?: JobOpeningStatus[];
    skip?: number;
    take?: number;
    searchText?: string;
    continuationToken?: string;
}

export interface JobOffer {
    url: string;
    status?: JobOfferStatus;
    jobOfferStatusReason?: JobOfferStatusReason;
    offerDate?: Date;
}

export enum JobOfferApplicationStatus {
    New = 0,
    OfferPrepared = 1,
}

export enum JobOfferApprovalStatus {
    NotStarted = 0,
    Approved = 1,
    Sendback = 2,
    SentForReview = 3,
    WaitingForReview = 4,
    Skipped = 5,
}

export enum JobOfferApprovalStatusReason {
    NotStarted = 0,
    Approved = 1,
    Sendback = 2,
    SentForReview = 3,
    WaitingForReview = 4,
    Skipped = 5,
}

export enum JobOfferArtifactDocumentType {
    Doc = 0,
    DocX = 1,
    Pdf = 2,
    Html = 3,
    Jpg = 4,
}

export enum JobOfferArtifactType {
    OfferLetter = 0,
    OtherDocument = 1,
    OfferLetterDocX = 2,
    OfferTemplate = 3,
}

export enum JobOfferArtifactUploadedBy {
    OfferManager = 0,
    Candidate = 1,
}

export enum JobOfferDeclineReason {
    CompensationPackage = 0,
    NotGoodCultureFit = 1,
    JobTitle = 2,
    Relocation = 3,
    WithdrawInterest = 4,
    AcceptedAnotherOffer = 5,
    AcceptedAnotherOfferWithYourCompany = 6,
    Other = 7,
}

export enum JobOfferEmailParticipantRole {
    HiringManager = 0,
    Recruiter = 1,
    Interviewer = 2,
    Contributor = 3,
    OfferCreator = 4,
    OfferRejector = 5,
}

export enum JobOfferFeatureName {
    ApprovalRequired = 0,
    ApprovalCommentRequired = 1,
    SequentialApproval = 2,
    ParallelApproval = 3,
    DeclineOffer = 4,
    OfferExpirationDate = 5,
    CustomRedirectUrl = 6,
    OnboardingRedirectRequired = 7,
}

export interface JobOfferHiringTeamView {
    jobApplicationID?: string;
    jobOfferID?: string;
    jobOfferStatus?: JobOfferStatus;
    jobOfferStatusReason?: JobOfferStatusReason;
    jobOfferPublishDate?: Date;
}

export enum JobOfferNonStandardReason {
    SalaryNegotiation = 0,
    BenefitsNegotiation = 1,
    LocationNegotiation = 2,
    CandidateInformation = 3,
    JobInformation = 4,
}

export enum JobOfferRole {
    Approver = 0,
    Fyi = 1,
    Owner = 2,
    CoAuthor = 3,
}

export enum JobOfferRuleAttributeDataType {
    FreeText = 0,
    NumericRange = 1,
    Clause = 2,
}

export enum JobOfferRuleParticipantRole {
    Owner = 0,
    Contributor = 1,
}

export enum JobOfferRuleProcessingStatus {
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Error = 3,
}

export enum JobOfferRuleStatus {
    Active = 0,
    Inactive = 1,
}

export enum JobOfferStatus {
    Active = 0,
    Inactive = 1,
    Pending = 2,
    Viewed = 3,
    Accepted = 4,
    Decline = 5,
}

export enum JobOfferStatusReason {
    New = 0,
    Draft = 1,
    Review = 2,
    Approved = 3,
    Published = 4,
    Accepted = 5,
    WithdrawnCandidateDispositioned = 6,
    NeedRevision = 7,
    WithdrawnOther = 8,
    Declined = 9,
    Closed = 10,
    Expired = 11,
}

export enum JobOfferTemplateAccessLevel {
    Org = 0,
    User = 1,
}

export enum JobOfferTemplatePackageStatus {
    Active = 0,
    Inactive = 1,
}

export enum JobOfferTemplatePackageStatusReason {
    Draft = 0,
    Published = 1,
    Versioned = 2,
}

export enum JobOfferTemplateRole {
    TemplateManager = 0,
    TemplateViewer = 1,
}

export enum JobOfferTemplateStatus {
    Active = 0,
    Archive = 1,
    Draft = 2,
    Inactive = 3,
}

export enum JobOfferTemplateStatusReason {
    Active = 0,
    Archive = 1,
    Draft = 2,
    Inactive = 3,
}

export enum JobOfferTokenDataType {
    FreeText = 0,
    NumericRange = 1,
    Clause = 2,
}

export enum JobOfferTokenType {
    SystemToken = 0,
    PlaceholderToken = 1,
    RulesetToken = 2,
}

export enum JobOpeningAssessmentRequirementStatus {
    NotRequired = 0,
    Required = 1,
}

export enum JobOpeningCareerLevel {
    Default = 0,
}

export enum JobOpeningExternalSource {
    Internal = 0,
    MSDataMall = 1,
    LinkedIn = 2,
    Greenhouse = 3,
    ICIMS = 4,
}

export enum JobOpeningIncentivePlan {
    Default = 0,
}

export enum JobOpeningIndustry {
    Default = 0,
}

export enum JobOpeningJobGrade {
    Default = 0,
}

export enum JobOpeningParticipantSource {
    Internal = 0,
    MSDataMall = 1,
    LinkedIn = 2,
    Greenhouse = 3,
    ICIMS = 4,
}

export interface JobOpeningPosition extends TalentBaseContract {
    jobOpeningPositionId?: string;
    jobId?: string;
    jobIds?: string[];
    title?: string;
    userAction?: UserAction;
    careerLevel?: string;
    costCenter?: string;
    sourcePositionNumber?: string;
    roleType?: string;
    incentivePlan?: string;
    jobGrade?: string;
    remunerationPeriod?: RenumerationPeriod;
    maximumRemuneration?: number;
    minimumRemuneration?: number;
    currencyCode?: CurrencyCode;
    department?: string;
    positionType?: JobOpeningPositionType;
    jobType?: string;
    jobFunction?: string;
    referenceJobOpeningIds?: string[];
    referenceApplicationIds?: string[];
    extendedAttributes?: { [key: string]: string; };
    reportsTo?: Worker;
    status?: JobPositionStatus;
    statusReason?: JobPositionStatusReason;
}

export interface JobOpeningPositionMetadata {
    jobOpeningPositions?: JobOpeningPosition[];
    total?: number;
    continuationToken?: string;
}

export interface JobOpeningPositionRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    continuationToken?: string;
}

export enum JobOpeningPositionType {
    FullTime = 0,
    PartTime = 1,
    Contract = 2,
    Temporary = 3,
    Volunteer = 4,
}

export interface JobOpeningProperties {
    addressBookTitle?: string;
    addressBookTitlePrefix?: string;
    addressBookTitleSuffix?: string;
    payScaleArea?: string;
    profession?: string;
    qualifier1?: string;
    qualifier2?: string;
    organization?: string;
    discipline?: string;
    standardTitle?: string;
    incentivePlan?: string;
    level?: string;
    careerStage?: string;
    roleType?: string;
    additionalWorkLocation?: string;
    primaryPositionID?: string;
    hireType?: string;
    adminOnboardingContact?: string;
    talentSourcer?: string;
}

export enum JobOpeningSeniorityLevel {
    Default = 0,
    Internship = 1,
    EntryLevel = 2,
    Associate = 3,
    MidSeniorLevel = 4,
    Director = 5,
    Executive = 6,
}

export interface JobOpeningStageActivityConfiguration {
    forCandidate?: boolean;
    allowAddingParticipants?: boolean;
}

export enum JobOpeningStatus {
    Active = 0,
    Closed = 1,
    Draft = 2,
    PendingForApproval = 3,
    Approved = 4,
    Rejected = 5,
}

export enum JobOpeningStatusReason {
    New = 0,
    Reactivated = 1,
    Filled = 2,
    Cancelled = 3,
}

export interface JobOpeningTemplateParticipant extends AADUser {
    userObjectId?: string;
    groupObjectId?: string;
    tenantObjectId?: string;
    isDefault?: boolean;
    canEdit?: boolean;
}

export enum JobOpeningTemplateSource {
    DefaultApplication = 0,
    Application = 1,
}

export enum JobOpeningVisibility {
    InternalOnly = 0,
    Public = 1,
}

export interface JobParticipant {
    FullName?: string;
    Alias?: string;
    EmailPrimary?: string;
    OfficeGraphIdentifier?: string;
    TeamsIdentifier?: string;
    Role?: JobParticipantRole;
}

export enum JobParticipantRole {
    HiringManager = 0,
    Recruiter = 1,
    Interviewer = 2,
    Contributor = 3,
    AA = 4,
    HiringManagerDelegate = 5,
}

export enum JobPermission {
    UpdateJobDetails = 0,
    UpdateJobProcess = 1,
    ActivateJob = 2,
    CreateJobApproval = 3,
    CreateJobPosting = 4,
    CloseJob = 5,
    CreateApplicant = 6,
    DeleteJob = 7,
    CreateHiringTeam = 8,
}

export enum JobPositionStatus {
    Active = 0,
    Inactive = 1,
}

export enum JobPositionStatusReason {
    Open = 0,
    Closed = 1,
}

export interface JobPostApplicationProfile {
    applicantProfile?: ApplicantProfile;
}

export enum JobPostStatus {
    Active = 0,
    Closed = 1,
}

export enum JobPostStatusReason {
    Active = 0,
    Closed = 1,
}

export enum JobPostSupplier {
    NotSpecified = 0,
    LinkedIn = 1,
    Monster = 2,
    Indeed = 3,
}

export interface JobRoles {
    jobParticipantRoles: JobParticipantRole[];
}

export interface JobSkill {
    skill?: string;
    score?: number;
}

export enum JobStage {
    Application = 0,
    Assessment = 1,
    Screening = 2,
    Interview = 3,
    Offer = 4,
    Prospect = 5,
}

export interface JobStageTemplate extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    ordinal?: number;
}

export interface JobTemplate extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    validFrom?: Date;
    validTo?: Date;
    isActive?: boolean;
    isDefault?: boolean;
    templateReference?: string;
    stageTemplates?: JobStageTemplate[];
}

export interface KoruAssessmentResultPayload {
    extraData?: any;
    webhookUrl?: string;
    processed?: boolean;
    tenantId?: string;
    scores?: string;
    candidate?: string;
    inProgress?: boolean;
    scored?: boolean;
    uuid?: string;
    initialized?: boolean;
    projectUuid?: string;
    profileUrl?: string;
}

export interface KoruAssessmentResultData {
    curiosity?: number;
    grit?: number;
    impact?: number;
    ownership?: number;
    polish?: number;
    rigor?: number;
    teamwork?: number;
    profileUrl?: string;
}

export enum KoruAssessmentStatus {
    Added = 1,
    Sent = 2,
    Started = 3,
    Submitted = 4,
    Done = 5,
}

export interface LinkedXRMEnvironmentInformation {
    instanceId: string;
    instanceUrl: string;
    instanceApiUrl: string;
    createdTime: Date;
    modifiedTime: Date;
    hostNameSuffix: string;
    localeId: number;
    initialUserObjectId: string;
    friendlyName: string;
    uniqueName: string;
    domainName: string;
    hasAttractServiceEndpointConfigured: boolean;
}

export interface LoginHint {
    obfuscatedUsername?: string;
    identityProvider?: string;
}

export interface MeetingAddress {
    type: string;
    postOfficeBox: string;
    street: string;
    city: string;
    state: string;
    countryOrRegion: string;
    postalCode: string;
}

export interface MeetingAttendee {
    type: string;
    emailAddress: MeetingAttendeeEmailAddress;
    status: MeetingAttendeeStatus;
    proposedNewTime?: MeetingTimeSpan;
}

export interface MeetingAttendeeEmailAddress {
    address: string;
    objectId?: string;
}

export interface MeetingAttendeeStatus {
    response: string;
    time: string;
}

export interface MeetingDateTime {
    dateTime: string;
    timeZone: string;
}

export interface MeetingDetails {
    id: string;
    subject?: string;
    description?: string;
    utcStart?: Date;
    utcEnd?: Date;
    location?: string;
    meetingLocation?: InterviewMeetingLocation;
    calendarEventId?: string;
    skypeOnlineMeetingRequired: boolean;
    onlineMeetingRequired: boolean;
    unknownFreeBusyTime?: boolean;
    status?: FreeBusyScheduleStatus;
    isTentative?: boolean;
    attendees: Attendee[];
    skypeOnlineMeeting?: SkypeSchedulerResponse;
    isDirty?: boolean;
    skypeMeetingType?: SkypeMeetingType;
    schedulerAccountEmail?: string;
    isPrivateMeeting?: boolean;
    isInterviewScheduleShared: boolean;
    isInterviewerNameShared: boolean;
    conference?: ConferenceInfo;
}

export interface MeetingInfo {
    id: string;
    userGroups: UserGroup;
    requester?: GraphPerson;
    scheduleEventId?: string;
    scheduleStatus?: ScheduleStatus;
    meetingDetails: MeetingDetails[];
    tenantId?: string;
    scheduleOrder?: number;
    InterviewerTimeSlotId: string;
}

export interface MeetingLocation {
    displayName: string;
    address: MeetingAddress;
}

export interface MeetingTimeSpan {
    start: MeetingDateTime;
    end: MeetingDateTime;
}

export interface OAuthToken {
    token_type: string;
    expires_in: number;
    access_token: string;
    id_token: string;
    refresh_token: string;
}

export interface OfferAcceptanceRedirectionSettings {
    webAddressUri?: string;
}

export enum OfferApplicationRole {
    OfferAdmin = 0,
}

export enum OfferArtifactDocumentType {
    Doc = 0,
    DocX = 1,
    Pdf = 2,
    Html = 3,
    Jpg = 4,
    XlsX = 5,
    PptX = 6,
    Jpeg = 7,
    Png = 8,
    Txt = 9,
}

export enum OfferArtifactType {
    OfferLetter = 0,
    OtherDocument = 1,
    OfferLetterDocX = 2,
    OfferTemplate = 3,
}

export enum OfferArtifactUploadedBy {
    OfferManager = 0,
    Candidate = 1,
}

export enum OfferAuthorEditMode {
    Edit = 0,
    ReadOnly = 1,
}

export interface OfferConfiguration {
    allowHiringManagerToPrepareOffer?: boolean;
    allowPositionReuseWithinJob?: boolean;
    launchOnboardApp?: boolean;
    manuallyRecordStatus?: boolean;
    sendMailToCandidate?: boolean;
}

export enum OfferDeclineReason {
    CompensationPackage = 0,
    NotGoodCultureFit = 1,
    JobTitle = 2,
    Relocation = 3,
    WithdrawInterest = 4,
    AcceptedAnotherOffer = 5,
    AcceptedAnotherOfferWithYourCompany = 6,
    Other = 7,
}

export interface OfferExpirySettings {
    isCustomDate?: boolean;
    expiryDays?: number;
}

export interface OfferFeature {
    offerFeature?: OfferFeatureName;
    isEnabled?: boolean;
}

export enum OfferFeatureName {
    ApprovalRequired = 0,
    ApprovalCommentRequired = 1,
    SequentialApproval = 2,
    ParallelApproval = 3,
    DeclineOffer = 4,
    OfferExpirationDate = 5,
    CustomRedirectUrl = 6,
    OnboardingRedirectRequired = 7,
}

export enum OfferNonStandardReason {
    SalaryNegotiation = 0,
    BenefitsNegotiation = 1,
    LocationNegotiation = 2,
    CandidateInformation = 3,
    JobInformation = 4,
}

export enum OfferParticipantRole {
    Approver = 0,
    Fyi = 1,
    Owner = 2,
    CoAuthor = 3,
}

export enum OfferParticipantStatus {
    NotStarted = 0,
    Approved = 1,
    Sendback = 2,
    SentForReview = 3,
    WaitingForReview = 4,
    Skipped = 5,
}

export enum OfferParticipantStatusReason {
    NotStarted = 0,
    Approved = 1,
    Sendback = 2,
    SentForReview = 3,
    WaitingForReview = 4,
    Skipped = 5,
}

export interface OfferSettings {
    offerFeature?: OfferFeature[];
    offerExpiry?: OfferExpirySettings;
    modifiedBy?: Person;
    modifiedDateTime?: Date;
    offerAcceptanceRedirectionSettings?: OfferAcceptanceRedirectionSettings;
}

export enum OfferStatus {
    Active = 0,
    Inactive = 1,
}

export enum OfferStatusReason {
    New = 0,
    Draft = 1,
    Review = 2,
    Approved = 3,
    Published = 4,
    Accepted = 5,
    WithdrawnCandidateDispositioned = 6,
    NeedRevision = 7,
    WithdrawnOther = 8,
    Declined = 9,
    Closed = 10,
    Expired = 11,
}

export enum OfferSyncbackAction {
    Create = 0,
    Update = 1,
    Delete = 2,
}

export enum OfferSyncbackMessage {
    OfferStatusChanged = 0,
}

export enum OfferTemplatePackageStatus {
    Active = 0,
    Inactive = 1,
}

export enum OfferTemplatePackageStatusReason {
    Draft = 0,
    Published = 1,
    Versioned = 2,
}

export enum OfferTemplateRole {
    TemplateManager = 0,
    TemplateViewer = 1,
}

export enum OfferTemplateRulesetFieldType {
    FreeText = 0,
    NumericRange = 1,
    Clause = 2,
}

export enum OfferTemplateStatus {
    Active = 0,
    Archive = 1,
    Draft = 2,
    Inactive = 3,
}

export enum OfferTemplateStatusReason {
    Active = 0,
    Archive = 1,
    Draft = 2,
    Inactive = 3,
}

export interface OfferUser {
    person: Person;
    roles: OfferApplicationRole[];
}

export interface OptionSetMetadata {
    values: OptionSetValue[];
}

export interface OptionSetValue {
    value?: number;
    label?: string;
    optionset?: string;
}

export interface Organization {
    displayName?: string;
}

export interface Organizer {
    emailAddress: MeetingAttendeeEmailAddress;
}

export interface PackageStatus {
    status: PackageStatusCode;
    installedVersion: string;
    packageName: string;
    details: string;
}

export enum PackageStatusCode {
    Unknown = 0,
    Installing = 1,
    Upgrading = 2,
    Completed = 3,
    Error = 4,
    NotStarted = 5,
}

export interface PaginationAndSortQueryParameters {
    pageNumber?: number;
    pageSize?: number;
    sortBy?: string;
    sortDirection?: string;
}

export enum PartyType {
    Person = 0,
    Organization = 1,
    Group = 2,
}

export interface PendingFeedback {
    StartDateTime?: Date;
    ModeOfInterview?: InterviewMode;
    PositionTitle?: string;
    InterviewerName?: string;
    CandidateName?: string;
    InterviewerOID?: string;
    jobApplicationID?: string;
    Roles?: JobParticipantRole[];
}

export enum Permission {
    None = 0,
    Read = 1,
    Write = 2,
}

export interface Person extends TalentBaseContract {
    objectId?: string;
    givenName?: string;
    middleName?: string;
    surname?: string;
    fullName?: string;
    email?: string;
    alternateEmail?: string;
    linkedIn?: string;
    linkedInAPI?: string;
    facebook?: string;
    twitter?: string;
    homePhone?: string;
    workPhone?: string;
    mobilePhone?: string;
    profession?: string;
    mailNickname?: string;
    externalWorkerId?: string;
    externalWorkerSource?: Source;
    mailingPostalAddress?: Address;
    otherPostalAddress?: Address;
}

export interface PersonName {
    GivenName?: string;
    MiddleName?: string;
    Surname?: string;
}

export interface PositionWorkerAssignment {
    jobPositionId?: string;
    mapId?: string;
    number?: string;
    workerId?: string;
}

export interface ProjectResponse {
    projectId?: string;
    resourceToken?: string;
}

export interface ProspectConfiguration {
    allowHiringManager?: boolean;
}

export interface RelevantJobApplications {
    applications?: Application[];
    total?: number;
    score?: number;
    lastUpdatedBy?: Date;
}

export interface RelevantProspects {
    prospects?: Applicant[];
    lastUpdatedBy?: Date;
}

export interface RemindScheduler {
    PositionTitle?: string;
    ExternalJobOpeningID?: string;
    SchedulerOID?: string;
    SchedulerName?: string;
    SchedulerEmail?: string;
}

export enum RenumerationPeriod {
    Weekly = 0,
    ByWeekly = 1,
    Monthly = 2,
}

export enum RequestStatus {
    Submitted = 0,
    Accepted = 1,
    Active = 2,
    Rejected = 3,
    Ended = 4,
    Revoked = 5,
}

export enum Required {
    ForAll = 0,
    ForInternalyOnly = 1,
    ForExternalOnly = 2,
    No = 3,
}

export interface ResponseStatus {
    response: string;
    time: Date;
}

export interface Room {
    name: string;
    address: string;
    status: InvitationResponseStatus;
}

export enum RulesetValidationResultCode {
    Ok = 0,
    InvalidNumericRange = 1,
    OutsideNumericRange = 2,
    UnknownNumericRange = 3,
    NonStandard = 4,
    UnknownHierarchy = 5,
    InactiveRuleset = 6,
    InvalidFields = 7,
    FieldsMismatch = 8,
    InvalidTokenValue = 9,
    Other = 10,
}

export interface Sample {
    deleteJobs?: boolean;
    job: Job;
    teams?: TeamMember[];
    candidates?: Applicant[];
    advanceStages?: { [key: string]: JobStage[]; };
    advanceStageOrder?: { [key: string]: number[]; };
    assessments?: { [key: string]: Feedback[]; };
    rejectedCandidates?: { [key: string]: JobApplicationStatusReasonPayload; };
}

export interface ScheduleAttendee {
    userId?: string;
    userName?: string;
    scheduleEventId?: string;
    responseStatus?: InvitationResponseStatus;
    startTime?: Date;
}

export interface ScheduleAvailability {
    id: string;
    startDate?: Date;
    endDate?: Date;
    isHiringTeamAvailable?: boolean;
    isCandidateAvailable?: boolean;
    timeZone?: string;
    userAction?: UserAction;
}

export interface ScheduleConfiguration {
    requestCandidateAvailability?: boolean;
    enableSkypeMeeting?: boolean;
    sendMailToCandidate?: boolean;
}

export interface ScheduleEmail {
    subject?: string;
    paragraph1?: string;
}

export interface ScheduleEmailTemplate {
    id?: string;
    tenantId?: string;
    userObjectId?: string;
    templateName?: string;
    templateType?: TemplateType;
    isDefault?: boolean;
    isAutosent?: boolean;
    isTentative?: boolean;
    ccEmailAddressRoles?: JobParticipantRole[];
    ccEmailAddressList?: string[];
    bccEmailAddressRoles?: JobParticipantRole[];
    bccEmailAddressList?: string[];
    primaryEmailRecipients?: string[];
    subject?: string;
    emailContent?: string;
    emailTokenList?: EmailTemplateTokens[];
    fromAddressMode?: SendEmailFromAddressMode;
}

export interface ScheduleEvent {
    id: string;
    candidate: GraphPerson;
    dates: string[];
    tenantId?: string;
    jobTitle?: string;
    userPermissions?: UserPermission[];
    deepLinkUrl?: string;
    timezone?: Timezone;
}

export interface ScheduleInvitationDetails {
    requester?: GraphPerson;
    primaryEmailRecipients: string[];
    ccEmailAddressList?: string[];
    bccEmailAddressList?: string[];
    subject: string;
    emailContent: string;
    emailAttachments?: FileAttachmentRequest;
    interviewDate?: string;
    startTime?: string;
    endTime?: string;
    location?: string;
    skypeMeetingUrl?: string;
    interviewTitle?: string;
    ScheduleId?: string;
    IsInterviewTitleShared?: boolean;
    IsInterviewScheduleShared?: boolean;
    IsInterviewerNameShared?: boolean;
}

export interface ScheduleInvitationDetailsV2 {
    requesterName?: string;
    requesterId?: string;
    requesterTitle?: string;
    requesterEmail?: string;
    requesterGivenName?: string;
    requesteMobilePhoner?: string;
    requesterOfficeLocation?: string;
    requesterPreferredLanguage?: string;
    requesterSurname?: string;
    requesterUserPrincipalName?: string;
    requesterInvitationResponseStatus?: string;
    requesterComments?: string;
    primaryEmailRecipients: string[];
    ccEmailAddressList?: string[];
    bccEmailAddressList?: string[];
    subject: string;
    emailContent: string;
    emailAttachmentFiles?: File[];
    emailAttachmentFileLabels?: string[];
    interviewDate?: string;
    startTime?: string;
    endTime?: string;
    location?: string;
    skypeMeetingUrl?: string;
    interviewTitle?: string;
    ScheduleId?: string;
    IsInterviewTitleShared?: boolean;
    IsInterviewScheduleShared?: boolean;
    IsInterviewerNameShared?: boolean;
}

export interface ScheduleInvitationRequest {
    subject: string;
    emailContent: string;
    emailAttachments?: FileAttachmentRequest;
    isInterviewTitleShared: boolean;
    isInterviewScheduleShared: boolean;
    primaryEmailRecipient: string;
    ccEmailAddressList?: string[];
    sharedSchedules?: CandidateScheduleCommunication[];
}

export interface ScheduleInvitationRequestV2 {
    subject: string;
    emailContent: string;
    emailAttachmentFiles?: File[];
    emailAttachmentFileLabels?: string[];
    isInterviewTitleShared: boolean;
    isInterviewScheduleShared: boolean;
    primaryEmailRecipient: string;
    ccEmailAddressList?: string[];
    sharedSchedulesScheduleId?: string[];
    sharedSchedulesIsInterviewScheduleShared?: boolean[];
    sharedSchedulesIsInterviewerNameShared?: boolean[];

}

export enum ScheduleStatus {
    NotScheduled = 0,
    Saved = 1,
    Queued = 2,
    Sent = 3,
    FailedToSend = 4,
    Delete = 5,
}

export interface SearchMetadataRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    searchFields?: string[];
    continuationToken?: string;
}

export interface SearchMetadataResponse {
    result?: any[];
    total?: number;
    continuationToken?: string;
}

export enum SendEmailFromAddressMode {
    SystemAccount = 0,
    CurrentUser = 1,
}

export interface SkypeEmbedded {
    onlineMeetingExtension: SkypeOnlineMeetingExtension[];
}

export interface SkypeHref {
    href: string;
}

export interface SkypeLinks {
    self: SkypeHref;
    textView: SkypeHref;
    htmlView: SkypeHref;
}

export enum SkypeMeetingType {
    None = 0,
    WithoutCodeEditor = 1,
    WithCodeEditor = 2,
}

export interface SkypeOnlineMeetingExtension {
    id: string;
    type: string;
    onlineMeetingConfLink: string;
    onlineMeetingExternalLink: string;
    onlineMeetingInternalLink: string;
    ucMeetingSetting: string;
    ucInband: string;
    ucCapabilities: string;
    participantPassCode: string;
    tollNumber: string;
    tollFreeNumber: string;
    _links: SkypeLinks;
}

export interface SkypeSchedulerResponse {
    joinUrl: string;
    onlineMeetingId: string;
    expirationTime: Date;
    subject: string;
    leaders: string[];
    attendees: string[];
    accessLevel: string;
    entryExitAnnouncement: string;
    automaticLeaderAssignment: string;
    onlineMeetingUri: string;
    organizerUri: string;
    phoneUserAdmission: string;
    lobbyBypassForPhoneUsers: string;
    etag: string;
    _links: SkypeLinks;
    _embedded: SkypeEmbedded;
}

export interface SocialIdentity {
    applicant?: Applicant;
    provider?: SocialNetworkProvider;
    providerMemberId?: string;
}

export enum SocialNetworkProvider {
    LinkedIn = 0,
    Facebook = 1,
    Twitter = 2,
}

export enum Source {
    Default = 0,
}

export interface StageActivity extends TalentBaseContract {
    id?: string;
    name?: string;
    displayName?: string;
    description?: string;
    ordinal?: number;
    subOrdinal?: number;
    audience?: ActivityAudience;
    isEnableForCandidate?: boolean;
    activityType?: JobApplicationActivityType;
    configuration?: string;
    scheduleEventId?: string;
    comment?: string;
    activityStatus?: JobApplicationActivityStatus;
    participants?: HiringTeamMember[];
    dueDateTime?: Date;
    plannedStartDateTime?: Date;
    required?: Required;
}

export interface StageScheduleEvent {
    JobApplicationId: string;
    TeamMembers: TeamMember[];
    Stage: JobStage;
    StageOrder?: number;
    Location?: string;
    Comment?: string;
    ScheduleEventId: string;
    ScheduleState: JobApplicationScheduleState;
    ScheduleType?: number;
    ScheduleAvailabilities?: ScheduleAvailability[];
    ScheduleDates?: string[];
    TimezoneName?: string;
}

export interface SuggestMeetingsRequest {
    interviewStartDateSuggestion: Date;
    interviewEndDateSuggestion: Date;
    timezone?: Timezone;
    panelInterview?: boolean;
    privateMeeting?: boolean;
    teamsMeeting?: boolean;
    meetingDurationInMinutes?: string;
    interviewersList?: UserGroup[];
    candidate?: GraphPerson;
}

export enum TalentApplicationRole {
    AttractAdmin = 0,
    OnboardAdmin = 1,
    LearnAdmin = 2,
    AttractRecruiter = 3,
    AttractHiringManager = 4,
    AttractReader = 5,
}

export interface TalentBaseContract {
    talentCustomFields: { [key: string]: any[]; };
}

export interface TalentEEOAccessTrail {
    toDate?: Date;
    fromDate?: Date;
    requestedOn?: Date;
    worker?: Person;
}

export interface TalentPool {
    poolId?: string;
    poolName?: string;
    description?: string;
    candidates?: Applicant[];
    candidateCount?: number;
    contributors?: TalentPoolParticipant[];
    lastModified?: Date;
    source?: TalentPoolSource;
    externalId?: string;
    userPermissions?: TalentPoolPermission[];
}

export interface TalentPoolCandidateRequest {
    skip?: number;
    take?: number;
    searchText?: string;
}

export interface TalentPoolCandidateResponse {
    candidates?: Applicant[];
    total?: number;
}

export interface TalentPoolMetadata {
    talentPools?: TalentPool[];
    total?: number;
}

export interface TalentPoolMetadaRequest {
    skip?: number;
    take?: number;
    searchText?: string;
    talentPoolRoles?: TalentPoolParticipantRole[];
}

export interface TalentPoolParticipant extends Person {
    role?: TalentPoolParticipantRole;
    userAction?: UserAction;
}

export enum TalentPoolParticipantRole {
    Owner = 0,
    Contributor = 1,
}

export enum TalentPoolPermission {
    ReadTalentPool = 0,
    UpdateTalentPool = 1,
    DeleteTalentPool = 2,
    AddCandidate = 3,
    DeleteCandidate = 4,
}

export enum TalentPoolSource {
    Internal = 0,
    LinkedIn = 2,
}

export interface TalentSource extends TalentBaseContract {
    id?: string;
    name?: string;
    domain?: string;
    description?: string;
    talentSourceCategory?: OptionSetValue;
    referalReference?: Person;
}

export enum TalentTagAssociationExternalSource {
    Internal = 0,
    MSDataMall = 1,
    Greenhouse = 2,
}

export interface TeamMember extends AADUser {
    Role: JobParticipantRole;
    UserAction: UserAction;
    IsMemberOfActivity: boolean;
}

export interface AADUser extends TalentBaseContract {
    name: string;
    id: string;
    title: string;
    email: string;
    given: string;
    mobilePhone: string;
    officeLocation: string;
    userPrincipalName: string;
    mailNickname?: string;
    externalWorkerId?: string;
    externalWorkerSource?: Source;
}

export interface TemplateSettings {
    disableTemplateCreation?: boolean;
    defaultTemplateIds?: string[];
    modifiedBy?: Person;
    modifiedDateTime?: Date;
}

export enum TemplateType {
    CandidateInterview = 0,
    CandidateInvite = 1,
    CandidateAvailability = 2,
    InterviewerMeeting = 3,
    InterviewerSummary = 4,
    InterviewerMeetingReminder = 5,
    InterviewerFeedbackReminder = 6,
    ReviewActivitiesTemplate = 7,
    OfferNotificationTemplate = 8,
    NewCandidateScheduler = 9,
    NotifySchedulerAcceptDecline = 10,
    RequestForFeedback = 11,
    RoomDeclineReminder = 12,
    PendingDeclinedResponseReminder = 13,
    ProspectCandidateApplyInvite = 14,
    InterviewInvitationFailed = 15,
    RequestForJobApproval = 16,
    NotifyJobApprovedToRequester = 17,
    NotifyJobDeniedToRequester = 18,
    RequestOfferApproval = 19,
    NotifyOfferApproved = 20,
    NotifyOfferRejected = 21,
    NotifyOfferPublishedToCandidate = 22,
    NotifyOfferWithdraw = 23,
    OfferExpiryReminder = 24,
    NotifyOfferDeclinedToRecruiter = 25,
    NotifyOfferDeclinedToHiringManager = 26,
    NotifyOfferAcceptedToRecruiter = 27,
    NotifyOfferAcceptedToHiringManager = 28,
    NotifyOfferExpired = 29,
    NotifyOfferApprovalToOfferCreator = 30,
    NotifyOfferRejectionToOfferCreator = 31,
    NotifyOfferCloseToRecruiter = 32,
    NotifyOfferCloseToHiringManager = 33,
    CandidateLoginFailed = 34,
    CandidateLoginInformation = 35,
    RequestForJobApprovalReminder = 36,
    NotifyApplicationRejectedToCandidate = 37,
}

export interface Timezone {
    timezoneName?: string;
    utcOffset?: number;
    utcOffsetHours?: string;
    timezoneAbbr?: string;
}

export enum TokenDataType {
    FreeText = 0,
    NumericRange = 1,
    Clause = 2,
}

export enum TokenType {
    SystemToken = 0,
    PlaceholderToken = 1,
    RulesetToken = 2,
}

export interface UpcomingInterviews {
    ScheduleForDay?: InterviewDetails[];
    ScheduledDatesForMonth?: Date[];
    SchedulesForMonth?: Date[];
}

export enum UserAction {
    None = 0,
    Add = 1,
    Update = 2,
    Delete = 3,
}

export interface UserESignConfiguration {
    id?: string;
    oid?: string;
    refreshToken?: string;
    esignTypeSelected?: ESignType;
    tenantId?: string;
    environmentId?: string;
    emailAddress?: string;
    apiAccessPoint?: string;
    webAccessPoint?: string;
}

export interface UserGroup {
    freeBusyTimeId?: string;
    users: GraphPerson[];
}

export interface UserPermission {
    user: GraphPerson;
    permission: Permission;
}

export interface UsersWithRolesSearchRequest {
    roles?: TalentApplicationRole[];
    skip?: number;
    take?: number;
    searchText?: string;
}

export interface UsersWithRolesSearchResponse {
    users?: UserWithRoles[];
    total?: number;
}

export interface UserWithRoles extends AADUser {
    roles?: TalentApplicationRole[];
}

export interface UserWithRolesTODO extends AADUser {
    roles?: string;
}

export interface WopiPreviewInfo {
    previewUrl: string;
    accessToken: string;
    accessTokenTtl: number;
}

export enum WorkerStatus {
    Active = 0,
    Inactive = 1,
}

export enum WorkerType {
    Employee = 0,
    Contractor = 1,
    Volunteer = 2,
    Unspecified = 3,
}

export interface WorkExperience extends TalentBaseContract {
    WorkExperienceId?: string;
    Title?: string;
    Organization?: string;
    Location?: string;
    Description?: string;
    IsCurrentPosition?: boolean;
    FromMonthYear?: Date;
    ToMonthYear?: Date;
}
