

#I @"..\JiraRestClient.Net\JiraRestClient.Net\bin\Debug\netstandard2.0\"
#r @"JiraRestClient.Net.dll"
#r @"nuget: Chiron.nstd20"
open System
open JiraRestClient.Net

let Uri = new Uri ("https://unidataplatform.atlassian.net/")
let AccountId = "712020:33dde142-de22-42ed-9b80-1fe1b22a57a5"
let Username = "anibal.yeh@uni-psg.com"
let Password = "ATATT3xFfGF07onk9pzRBkFPv9s3PiXYJ2vQZvjp-ThR6tJeVI98IzTcuXRWoQmF_jJulf3J9T-qx_rKCaoIfg5LDBepTdXORzYix97PUmilVXdjyjGgzSPH3QGnoMKm1HY6tuAORw_UNBop7MwbNdY0f9eCs4wHwLVeHS50SWwyYUkIGYBHUoY=2AF2EC3F"
let ProjectKey = "ST"

let RestClient = new JiraRestClient(Uri, Username, Password)
let token = RestClient.Login(Username, Password)



open Chiron.nstd20
open System
open System.IO
open System.Collections
open System.Collections.Concurrent
open System.Collections.Generic
open System.Reflection
open Microsoft.FSharp.Collections


let typeAliases = dict [
    typeof<int>     , "int"
    typeof<string>  , "string"
    typeof<float>   , "float"
    typeof<decimal> , "decimal"
    typeof<bool>    , "bool"
    typeof<obj>     , "obj"
    // Add more type mappings as needed
]

let rec typeName (t: Type) : string =
    match typeAliases.TryGetValue(t) with
    | true, aliasName -> aliasName
    | _ ->
        if t.IsGenericType then
            let genericTypeName = t.GetGenericTypeDefinition().Name
            let backtickIndex = genericTypeName.IndexOf('`')
            let cleanName = if backtickIndex > 0 then genericTypeName.[..backtickIndex - 1] else genericTypeName
            let typeArgs = t.GetGenericArguments() |> Array.map typeName
            match cleanName, typeArgs with
            | "FSharpList", [|arg|] -> sprintf "%s list" arg
            | "FSharpOption", [|arg|] -> sprintf "%s option" arg
            | _, _ -> sprintf "%s<%s>" cleanName (String.Join(", ", typeArgs))
        elif t.IsArray then
            sprintf "%s[]" (typeName <| t.GetElementType())
        //elif t.GetGenericTypeDefinition() = typedefof<List<_>> then
        //    "list"
        //elif t.GetGenericTypeDefinition() = typeof<Option<_>> then
        //    "option"
        else t.Name

let rec isCollectionType (t: Type) : bool =
    (typeof<IEnumerable>.IsAssignableFrom(t) && t <> typeof<string>) || t.IsGenericType && (t.GetGenericTypeDefinition() = typedefof<List<_>>)

let rec clearTypeName (obj: obj) : string =
    let t = obj.GetType()
    if isCollectionType t then
        if t.IsArray then
            printfn $"----> CollectionType IsArray {t.GetType().Name}"
            let genericArg = t.GetElementType()
            sprintf "%s []" (typeName genericArg)
        elif t.GetGenericTypeDefinition() = typedefof<List<_>> then
            let genericArg = t.GetGenericArguments().[0]
            sprintf "%s list" (typeName genericArg)
        else
            printfn "----> CollectionType but not resolved"
            typeName t
    else
        match obj with
        | :? Json as j -> 
            Json.jsonValue j |> clearTypeName
        | _ ->
            printfn "----> Not collection or Json Type"
            typeName t

// Example usage:
let dict_ = ConcurrentDictionary<int, string>()
printfn "%s" (clearTypeName dict_)  // Returns "Dictionary<int, string>"

let list_ = [1; 2; 3; 4; 5] // F# List
printfn "%s" (clearTypeName list_)  // Returns "int list"

let array_ = [|1; 2; 3; 4; 5|] // Array
printfn "%s" (clearTypeName array_) // Returns "int[]"


isCollectionType (array_.GetType())
[|123|].GetType().GetGenericArguments()

let trello = File.ReadAllText @"C:\Users\Administrator\Desktop\New folder\s2SPFVR9.json"
let trello2 = File.ReadAllText @"C:\Users\Administrator\Desktop\New folder\7xvXay78.json"

let (JPass jt) = Json.parse trello

let keys = 
    let (Object (JsonObject.ReadObject (jo,_))) = jt
    (*
    jo 
    |> List.map fst
    |> List.iter (printfn "%s")
    *)

    jo 
    |> List.map (fun (k, v) ->
        let jv = Json.jsonValue v
        if jv <> null then
            printfn $"{k}: {clearTypeName jv}"
        else
            printfn $"{k}: obj option"
    )

    (Json.jsonValue ((Map jo)["checklists"])).GetType().FullName

    ((Json.jsonValue ((Map jo)["checklists"])) :?> obj[])[0]

    clearTypeName ((Map jo)["checklists"])

    (Json.jsonValue ((Map jo)["checklists"])).GetType().GetElementType().Name
    (Json.jsonValue ((Map jo)["checklists"])).GetType().Name
    

    ((Map jo)["checklists"]).GetType().Name


type CheckItemLimits = {
    mutable status: string
    mutable disableAt: int
    mutable warnAt: int
}

// Define a type for the limits within a checklist
type Limits = {
    mutable perChecklist: CheckItemLimits option
}

type TextData = {
    mutable emoji: Map<string, obj> //Choice<Map<string, string>,string>
}


// Define a type for an individual check item
type CheckItem = {
    mutable id: string
    mutable name: string
    mutable nameData: obj option // Assuming this can be null, representing with an option type
    mutable pos: int
    mutable state: string
    mutable due: string option // Assuming this can be null, representing with an option type
    mutable dueReminder: string option // Assuming this can be null, representing with an option type
    mutable idMember: string option // Assuming this can be null, representing with an option type
    mutable idChecklist: string
    mutable textData: obj option
}

// Define a type for the main checklist item, which includes other records
type ChecklistItem = {
    mutable id: string
    mutable name: string
    mutable idBoard: string
    mutable idCard: string
    mutable pos: int
    mutable limits: Limits
    mutable checkItems: CheckItem list // A list of CheckItem records
    mutable creationMethod: string option // Assuming this can be null, representing with an option type
}


type NonPublicInfo = obj

// Define the main Member type
type Member = {
    mutable id: string
    mutable aaId: string
    mutable activityBlocked: bool
    mutable avatarHash: string option // Assuming this can be null, represented as an option type
    mutable avatarUrl: string option // Assuming this can be null, represented as an option type
    mutable bio: string option // Assuming this can be empty, represented as an option type
    mutable bioData: obj option // Assuming this can be null, represented as an option type
    mutable confirmed: bool
    mutable fullName: string
    mutable idEnterprise: string option // Assuming this can be null, represented as an option type
    mutable idEnterprisesDeactivated: string list option // Assuming this can be null or empty, represented as an option type
    mutable idMemberReferrer: string option // Assuming this can be null, represented as an option type
    mutable idPremOrgsAdmin: string list // Assuming this can be empty, represented directly as a list
    mutable initials: string
    mutable memberType: string
    mutable nonPublic: NonPublicInfo option // Assuming this can be null or undefined details, represented as an option type
    mutable nonPublicAvailable: bool
    mutable products: string list // Assuming this can be empty, represented directly as a list
    mutable url: string
    mutable username: string
    mutable status: string
}

type LimitSettings = {
    mutable status: string
    mutable disableAt: int
    mutable warnAt: int
}

// Define types for different categories of limits
type AttachmentLimits = {
    mutable perBoard: LimitSettings
    mutable perCard: LimitSettings
}

type BoardLimits = {
    mutable totalMembersPerBoard: LimitSettings
    mutable totalAccessRequestsPerBoard: LimitSettings
}

type CardLimits = {
    mutable openPerBoard: LimitSettings
    mutable openPerList: LimitSettings
    mutable totalPerBoard: LimitSettings
    mutable totalPerList: LimitSettings
}

type ChecklistLimits = {
    mutable perBoard: LimitSettings
    mutable perCard: LimitSettings
}

type perCheckItemLimits = {
    mutable perChecklist: LimitSettings
}

type CustomFieldLimits = {
    mutable perBoard: LimitSettings
}

type CustomFieldOptionLimits = {
    mutable perField: LimitSettings
}

type LabelLimits = {
    mutable perBoard: LimitSettings
}

type ListLimits = {
    mutable openPerBoard: LimitSettings
    mutable totalPerBoard: LimitSettings
}

type StickerLimits = {
    mutable perCard: LimitSettings
}

type ReactionLimits = {
    mutable perAction: LimitSettings
    mutable uniquePerAction: LimitSettings
}

// Define a type for the overall settings
type ResourceLimits = {
    mutable attachments: AttachmentLimits
    mutable boards: BoardLimits
    mutable cards: CardLimits
    mutable checklists: ChecklistLimits
    mutable checkItems: perCheckItemLimits option
    mutable customFields: CustomFieldLimits
    mutable customFieldOptions: CustomFieldOptionLimits
    mutable labels: LabelLimits
    mutable lists: ListLimits
    mutable stickers: StickerLimits
    mutable reactions: ReactionLimits option
}

type CardDescription = {
    mutable desc: string option
    mutable id: string
    mutable name: string
    mutable idShort: int
    mutable shortLink: string
}

type OldDescription = {
    mutable desc: string option
}

type BoardDetails = {
    mutable id: string
    mutable name: string
    mutable shortLink: string
}

type ListDetails = {
    mutable id: string
    mutable name: string option
}

type ChecklistReference = {
    mutable id: string
    mutable name: string
}


type CheckItemReference = {
    mutable id: string
    mutable name: string
    mutable state: string
    mutable textData: obj option
}

type DataDetails = {
    mutable card: CardDescription option
    mutable old: OldDescription option
    mutable board: BoardDetails option
    mutable list: ListDetails option
    mutable text: string option
    mutable textData: obj option
    mutable checklist: ChecklistReference option
    mutable checkItem: CheckItemReference option
}

type MemberCreatorDetails = {
    mutable id: string
    mutable activityBlocked: bool
    mutable avatarHash: string option // Assuming this can be null, represented as an option type
    mutable avatarUrl: string option // Assuming this can be null, represented as an option type
    mutable fullName: string
    mutable idMemberReferrer: string option // Assuming this can be null, represented as an option type
    mutable initials: string
    mutable nonPublic: Map<string, obj> // Assuming this can be an empty object, use Map for flexibility
    mutable nonPublicAvailable: bool
    mutable username: string
}

type AppCreator = {
    id:string
}

type ActivityRecord = {
    mutable id: string
    mutable idMemberCreator: string
    mutable data: DataDetails
    mutable appCreator: AppCreator option // Assuming this can be null, represented as an option type
    mutable ``type``: string
    mutable date: string
    mutable limits: Map<string, obj> option // Assuming this can be null, use Map for flexibility if the structure is unknown or variable
    mutable memberCreator: MemberCreatorDetails
}

type TrelloAttachmentCounts = {
    mutable board: int
    mutable card: int
}

type AttachmentByType = {
    mutable trello: TrelloAttachmentCounts
}

type BadgeDetails = {
    mutable attachmentsByType: AttachmentByType
    mutable location: bool
    mutable votes: int
    mutable viewingMemberVoted: bool
    mutable subscribed: bool
    mutable fogbugz: string
    mutable checkItems: int
    mutable checkItemsChecked: int
    mutable checkItemsEarliestDue: string option // Assuming this can be null, represented as an option type
    mutable comments: int
    mutable attachments: int
    mutable description: bool
    mutable due: string option // Assuming this can be null, represented as an option type
    mutable dueComplete: bool
    mutable start: string option // Assuming this can be null, represented as an option type
}

type LimitPerCard = {
    mutable status: string option
    mutable disableAt: int option
    mutable warnAt: int option
}

type CardLimits2 = {
    mutable attachments: LimitPerCard option
    mutable checklists: LimitPerCard option
    mutable stickers: LimitPerCard option
}

type LabelDetails = {
    mutable id: string
    mutable idBoard: string
    mutable name: string
    mutable color: string
    mutable uses: int
}

type AttachmentDetails = {
    mutable id: string
    mutable bytes: int option // Assuming this can be null, represented as an option type
    mutable date: string
    mutable edgeColor: string option // Assuming this can be null, represented as an option type
    mutable idMember: string
    mutable isUpload: bool
    mutable mimeType: string option // Assuming this can be null, represented as an option type
    mutable name: string
    mutable previews: list<obj> // List of previews, assuming string URLs (adjust if the structure is different)
    mutable url: string
    mutable pos: int
    mutable fileName: string option // Assuming this can be null, represented as an option type
}

type CoverDetails = {
    mutable idAttachment: string option // Assuming this can be null, represented as an option type
    mutable color: string option // Assuming this can be null, represented as an option type
    mutable idUploadedBackground: string option // Assuming this can be null, represented as an option type
    mutable size: string option // Assuming this can have predefined values or be null
    mutable brightness: string option // Assuming this can have predefined values or be null
    mutable idPlugin: string option // Assuming this can be null, represented as an option type
}

type CardDetails = {
    mutable id: string
    mutable address: string option // Assuming this can be null, represented as an option type
    mutable badges: BadgeDetails
    mutable checkItemStates: list<obj> // Assuming list of strings, adjust if the structure is different
    mutable closed: bool
    mutable coordinates: string option // Assuming this can be null, represented as an option type
    mutable creationMethod: string option // Assuming this can be null, represented as an option type
    mutable dueComplete: bool
    mutable dateLastActivity: string
    mutable desc: string
    mutable descData: obj option // Assuming this can be null, represented as an option type
    mutable due: string option // Assuming this can be null, represented as an option type
    mutable dueReminder: int option // Assuming this can be null, represented as an option type
    mutable email: string option // Assuming this can be null, represented as an option type
    mutable idBoard: string
    mutable idChecklists: list<string> // List of checklist ids
    mutable idLabels: list<string> // List of label ids
    mutable idList: string
    mutable idMembers: list<string> // List of member ids
    mutable idMembersVoted: list<string> // List of ids for members voted
    mutable idOrganization: string
    mutable idShort: int
    mutable idAttachmentCover: string option // Assuming this can be null, represented as an option type
    mutable labels: list<LabelDetails>
    mutable limits: CardLimits2
    mutable locationName: string option // Assuming this can be null, represented as an option type
    mutable manualCoverAttachment: bool
    mutable name: string
    mutable nodeId: string
    mutable pos: int
    mutable shortLink: string
    mutable shortUrl: string
    mutable staticMapUrl: string option // Assuming this can be null, represented as an option type
    mutable start: string option // Assuming this can be null, represented as an option type
    mutable subscribed: bool
    mutable url: string
    mutable cover: CoverDetails // Assume CoverDetails is previously defined; if not, define similarly
    mutable isTemplate: bool
    mutable cardRole: string option // Assuming this can be null, represented as an option type
    mutable attachments: list<AttachmentDetails>
    mutable pluginData: list<string> // Assuming list of strings, adjust if the structure is different
    mutable customFieldItems: list<string> // Assuming list of strings, adjust if the structure is different
}

type Label = {
    mutable id: string
    mutable idBoard: string
    mutable name: string
    mutable color: string
    mutable uses: int
}

type ListLimitDetails = {
    mutable status: string
    mutable disableAt: int
    mutable warnAt: int
}

type tListLimits = {
    mutable openPerList: ListLimitDetails option
    mutable totalPerList: ListLimitDetails option
}

type TrelloList = {
    mutable id: string
    mutable name: string
    mutable closed: bool
    mutable color: string option // Assuming this can be null, represented as an option type
    mutable idBoard: string
    mutable pos: int
    mutable subscribed: bool
    mutable softLimit: string option // Assuming this can be null, represented as an option type
    mutable creationMethod: string option // Assuming this can be null, represented as an option type
    mutable idOrganization: string
    mutable limits: tListLimits
    mutable nodeId: string
}

type Membership = {
    mutable idMember: string
    mutable memberType: string
    mutable unconfirmed: bool
    mutable deactivated: bool
    mutable id: string
}

type PluginDataValue = {
    mutable dateAutoEnabled: int64 option // Assuming this can vary, use option type if uncertain.
}

type PluginData = {
    mutable id: string
    mutable idPlugin: string
    mutable scope: string
    mutable idModel: string
    mutable value: string // JSON string since F# doesn't directly handle nested arbitrary JSON in types; you'll parse this separately.
    mutable access: string
}

type BackgroundImageScaled = {
    mutable width: int
    mutable height: int
    mutable url: string
}

type SwitcherView = {
    mutable viewType: string
    mutable enabled: bool
}

type Prefs = {
    mutable permissionLevel: string
    mutable hideVotes: bool
    mutable voting: string
    mutable comments: string
    mutable invitations: string
    mutable selfJoin: bool
    mutable cardCovers: bool
    mutable cardCounts: bool
    mutable isTemplate: bool
    mutable cardAging: string
    mutable calendarFeedEnabled: bool
    mutable hiddenPluginBoardButtons: list<string>
    mutable switcherViews: list<SwitcherView>
    mutable background: string
    mutable backgroundColor: string option
    mutable backgroundImage: string option
    mutable backgroundTile: bool
    mutable backgroundBrightness: string
    mutable backgroundImageScaled: list<BackgroundImageScaled>
    mutable backgroundBottomColor: string
    mutable backgroundTopColor: string
    mutable canBePublic: bool
    mutable canBeEnterprise: bool
    mutable canBeOrg: bool
    mutable canBePrivate: bool
    mutable canInvite: bool
}

type LabelNames = {
    mutable green: string
    mutable yellow: string
    mutable orange: string
    mutable red: string
    mutable purple: string
    mutable blue: string
    mutable sky: string
    mutable lime: string
    mutable pink: string
    mutable black: string
    mutable green_dark: string
    mutable yellow_dark: string
    mutable orange_dark: string
    mutable red_dark: string
    mutable purple_dark: string
    mutable blue_dark: string
    mutable sky_dark: string
    mutable lime_dark: string
    mutable pink_dark: string
    mutable black_dark: string
    mutable green_light: string
    mutable yellow_light: string
    mutable orange_light: string
    mutable red_light: string
    mutable purple_light: string
    mutable blue_light: string
    mutable sky_light: string
    mutable lime_light: string
    mutable pink_light: string
    mutable black_light: string
}


type Trello = {
    mutable id: string
    mutable nodeId: string
    mutable name: string
    mutable desc: string
    mutable descData: obj option
    mutable closed: bool
    mutable dateClosed: obj option
    mutable idOrganization: string
    mutable idEnterprise: obj option
    mutable limits: ResourceLimits option
    mutable pinned: bool
    mutable starred: bool
    mutable url: string
    mutable prefs: Prefs
    mutable shortLink: string
    mutable subscribed: bool
    mutable labelNames: LabelNames
    mutable powerUps: obj []
    mutable dateLastActivity: string
    mutable dateLastView: string
    mutable shortUrl: string
    mutable idTags: obj []
    mutable datePluginDisable: obj option
    mutable creationMethod: string
    mutable ixUpdate: string
    mutable templateGallery: obj option
    mutable enterpriseOwned: bool
    mutable idBoardSource: obj option
    mutable premiumFeatures: string []
    mutable idMemberCreator: string
    mutable actions: ActivityRecord []
    mutable cards: CardDetails []
    mutable labels: Label []
    mutable lists: TrelloList []
    mutable members: Member []
    mutable checklists: ChecklistItem []
    mutable customFields: obj []
    mutable memberships: Membership []
    mutable pluginData: PluginData []
}


let initCheckItem () : CheckItem = {
    id = ""
    name = ""
    nameData = NonedescData
    pos = 0
    state = ""
    due = None
    dueReminder = None
    idMember = None
    idChecklist = ""
}

let initChecklistItem () : ChecklistItem = {
    id = ""
    name = ""
    idBoard = ""
    idCard = ""
    pos = 0
    limits = { perChecklist = { status = ""; disableAt = 0; warnAt = 0 } }
    checkItems = List.empty
    creationMethod = None
}

let initMember () : Member = {
    id = ""
    aaId = ""
    activityBlocked = false
    avatarHash = None
    avatarUrl = None
    bio = None
    bioData = None
    confirmed = true
    fullName = ""
    idEnterprise = None
    idEnterprisesDeactivated = None
    idMemberReferrer = None
    idPremOrgsAdmin = List.empty
    initials = ""
    memberType = ""
    nonPublic = None
    nonPublicAvailable = false
    products = List.empty
    url = ""
    username = ""
    status = ""
}



let initLimitSettings () : LimitSettings = {
    status = ""
    disableAt = 0
    warnAt = 0
}

let initAttachmentLimits () : AttachmentLimits = {
    perBoard = initLimitSettings ()
    perCard = initLimitSettings ()
}

let initBoardLimits () : BoardLimits = {
    totalMembersPerBoard = initLimitSettings ()
    totalAccessRequestsPerBoard = initLimitSettings ()
}

let initCardLimits () : CardLimits = {
    openPerBoard = initLimitSettings ()
    openPerList = initLimitSettings ()
    totalPerBoard = initLimitSettings ()
    totalPerList = initLimitSettings ()
}

let initChecklistLimits () : ChecklistLimits = {
    perBoard = initLimitSettings ()
    perCard = initLimitSettings ()
}

let initCustomFieldLimits () : CustomFieldLimits = {
    perBoard = initLimitSettings ()
}

let initCustomFieldOptionLimits () : CustomFieldOptionLimits = {
    perField = initLimitSettings ()
}

let initLabelLimits () : LabelLimits = {
    perBoard = initLimitSettings ()
}

let initListLimits () : ListLimits = {
    openPerBoard = initLimitSettings ()
    totalPerBoard = initLimitSettings ()
}

let initStickerLimits () : StickerLimits = {
    perCard = initLimitSettings ()
}

let initReactionLimits () : ReactionLimits = {
    perAction = initLimitSettings ()
    uniquePerAction = initLimitSettings ()
}

let initResourceLimits () : ResourceLimits = {
    attachments = initAttachmentLimits ()
    boards = initBoardLimits ()
    cards = initCardLimits ()
    checklists = initChecklistLimits ()
    checkItems = { perChecklist = initLimitSettings () }
    customFields = initCustomFieldLimits ()
    customFieldOptions = initCustomFieldOptionLimits ()
    labels = initLabelLimits ()
    lists = initListLimits ()
    stickers = initStickerLimits ()
    reactions = initReactionLimits ()
}

let initTrello () : Trello = {
    id = ""
    nodeId = ""
    name = ""
    desc = ""
    descData = None
    closed = false
    dateClosed = None
    idOrganization = ""
    idEnterprise = None
    limits = initResourceLimits ()
    pinned = false
    starred = false
    url = ""
    prefs = Unchecked.defaultof<_>
    shortLink = ""
    subscribed = false
    labelNames = Unchecked.defaultof<_>
    powerUps = [||]
    dateLastActivity = ""
    dateLastView = ""
    shortUrl = ""
    idTags = [||]
    datePluginDisable = None
    creationMethod = ""
    ixUpdate = ""
    templateGallery = None
    enterpriseOwned = false
    idBoardSource = None
    premiumFeatures = [||]
    idMemberCreator = ""
    actions = [||]
    cards = [||]
    labels = [||]
    lists = [||]
    members = [||]
    checklists = [||]
    customFields = [||]
    memberships = [||]
    pluginData = [||]
}

#r @"nuget: Newtonsoft.Json"

open Newtonsoft.Json


open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.IO

open Newtonsoft.Json.Linq

// Function to update all empty "emoji" objects with a sequential number
let updateEmojiWithSerial (jsonContent: string) : string =
    let parsedJson = JToken.Parse(jsonContent)
    let mutable counter = 1
    
    let rec updateEmojis (token:JToken) =
        match token.Type with
        | JTokenType.Object ->
            let obj = token :?> JObject
            if obj.ContainsKey("emoji") && obj["emoji"].Type = JTokenType.Object then
                let emojiObj = obj["emoji"] :?> JObject
                if emojiObj.Count = 0 then  // Check if emoji is an empty object
                    emojiObj["v"] <- JToken.FromObject(sprintf "%i" counter)
                    counter <- counter + 1
            for prop in obj.Properties() do
                updateEmojis prop.Value
        | JTokenType.Array ->
            for item in token :?> JArray do
                updateEmojis item
        | _ -> () // For other types, do nothing
    
    // Start updating emojis from the root token
    updateEmojis parsedJson

    // Return the updated JSON string
    parsedJson.ToString(Newtonsoft.Json.Formatting.Indented)

// This function remains unchanged
let updateEmojiInFile (filePath: string) =
    let jsonContent = File.ReadAllText(filePath)
    let updatedContent = updateEmojiWithSerial jsonContent
    File.WriteAllText(filePath, updatedContent)




updateEmojiInFile @"C:\Users\Administrator\Desktop\New folder\s2SPFVR9.json"
updateEmojiInFile @"C:\Users\Administrator\Desktop\New folder\7xvXay78.json"










let deserializeCheckItem (json: string) : CheckItem =
    JsonConvert.DeserializeObject<CheckItem>(json)

let deserializeChecklistItem (json: string) : ChecklistItem =
    JsonConvert.DeserializeObject<ChecklistItem>(json)

let deserializeMember (json: string) : Member =
    JsonConvert.DeserializeObject<Member>(json)

// Continue for other types as needed

let deserializeTrello (json: string) : Trello =
    JsonConvert.DeserializeObject<Trello>(json)



let tObj = deserializeTrello trello


#r @"nuget: FSharp.Json"


open FSharp.Json

let config = JsonConfig.create(allowUntyped = true)

let deserialized = Json.deserializeEx<Trello> config trello

//Json.deserializeEx<Map<string, string>> config "{}"


//Json.serializeEx config td

open System.Text.RegularExpressions
let regex = Regex "((14036)|(15122))"


open JiraRestClient.Net.Domain.Issue
open JiraRestClient.Net.Domain
open System.Collections.Generic
let [|c1; c2|] =
    deserialized.cards 
    |> Array.filter (fun c ->
        regex.IsMatch c.name
    )
    //|> Array.length


let c = c1

let issueUpdate = IssueUpdate ()

let f = IssueFields()

let contentList = new List<Content> ()
let contentListParag = new List<Content> ()

let txt = Content(Type = "text",Text = c.desc)
contentList.Add txt

let paragraph = Content(Type = "paragraph", Contents = contentList)
contentListParag.Add paragraph

let desc = Description(Version = 1, Type = "doc", Content = contentListParag) 

f.Description <- desc
f.Summary <- c.name
f.Project <- Project(Key="ST")
f.Issuetype <- IssueType(Name = "Story")
f.StoryPoint <- 0.0f
issueUpdate.Fields <- f


let issueResponse = RestClient.IssueClient.CreateIssue(issueUpdate)



//let userClient = RestClient.UserClient
//let user = userClient.GetUserByUsername(AccountId)




let c = c2

let issueUpdate2 = IssueUpdate ()

let f2 = IssueFields()

let contentList2 = new List<Content> ()
let contentListParag2 = new List<Content> ()

let txt2 = Content(Type = "text",Text = c.desc)
contentList2.Add txt2

let paragraph2 = Content(Type = "paragraph", Contents = contentList)
contentListParag2.Add paragraph2

let desc2 = Description(Version = 1, Type = "doc", Content = contentListParag2) 

f2.Description <- desc2
f2.Summary <- c2.name
f2.Project <- Project(Key="ST")
f2.Issuetype <- IssueType(Name = "Story")
f2.StoryPoint <- 0.0f
issueUpdate2.Fields <- f2


let issueResponse2 = RestClient.IssueClient.CreateIssue(issueUpdate2)
issueResponse2.Errors

c1.idChecklists

let c1CheckList = 
    deserialized.checklists |> Array.filter (fun cl ->
        c1.idChecklists |> List.contains cl.id
    )


let subTasks =
    c1CheckList[0].checkItems
    |> List.map (fun cl ->
        let issueUpdate = IssueUpdate ()

        let f = IssueFields()

        let parent = Issue(Key=issueResponse.Key)
        f.Parent <- parent

        let contentList = new List<Content> ()
        let contentListParag = new List<Content> ()

        let txt = Content(Type = "text",Text = "")
        contentList.Add txt

        let paragraph = Content(Type = "paragraph", Contents = contentList)
        contentListParag.Add paragraph

        let desc = Description(Version = 1, Type = "doc", Content = contentListParag) 

        f.Description <- desc
        f.Summary <- cl.name
        f.Project <- Project(Key="ST")
        f.Issuetype <- IssueType(Name = "Sub-task")
        //f.StoryPoint <- 0.0f
        issueUpdate.Fields <- f


        RestClient.IssueClient.CreateIssue(issueUpdate)


    )

subTasks[0].Errors
c1CheckList.Length