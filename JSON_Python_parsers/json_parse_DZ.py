"""
by Matt Cabanag, 2019 - 2024

Helper script to parse Firebase.Json into CSV

"""


import os
import json

#====[FILENAME]====
inputJSON = "c2sim-DZ.json"
#inputJSON = "mattc-c2sim-studies-x1-2-2024-03-22.json"
outputSuffix = ""
#inputJSON = "one.json"

#====[OUTPUT SWITCHES]====
L337_MODE = False
FLAT_MODE = True
VALID_TRIAL_COUNT = 72
TRIAL_SURVEY_START_INDEX = 3
SURVEY_BLOCKS = 3
SURVEYS_PER_BLOCK = 2

path = os.getcwd()
print(path+"\\"+inputJSON)


with open(path+"\\"+inputJSON, "r") as read_file:
    data = json.load(read_file)

####
#  For interpreting scene names like "G_DZDI5_W12" :->
#  Change x_INDEX as required
####
#====[DISPLAY MODES]====
DISPLAY_MODES = {}
DISPLAY_MODES["T"] = "Text"
DISPLAY_MODES["G"] = "Graphical"
DISPLAY_MODES["X"] = "Text+Graphical"
DISPLAY_MODE_INDEX = 0

#====[TASK PROFILES]====
TASK_PROFILE_INDEX = 1

TASK_PROFILES = {}
TASK_PROFILES["CC5"] = "Collision Avoidance"
TASK_PROFILES["NoCC"] = "Collision Avoidance"
TASK_PROFILES["CCD"] = "Collision Avoidance"
TASK_PROFILES["CCD5"] = "Collision Avoidance"
TASK_PROFILES["FCC5"] = "Collision Avoidance"
TASK_PROFILES["FCCD5"] = "Collision Avoidance"
TASK_PROFILES["FlatCC"] = "Collision Avoidance"
TASK_PROFILES["FlatNoCC"] = "Collision Avoidance"
TASK_PROFILES["OverflyCC"] = "Collision Avoidance"
TASK_PROFILES["FCC"] = "Collision Avoidance"

TASK_PROFILES["DZDI5"] = "Danger Zone"
TASK_PROFILES["DZNA5"] = "Danger Zone"
TASK_PROFILES["DZSI5"] = "Danger Zone"
TASK_PROFILES["DZUA5"] = "Danger Zone"

TASK_PROFILES["DZNA"] = "Danger Zone"
TASK_PROFILES["DZUA"] = "Danger Zone"
TASK_PROFILES["DZSI"] = "Danger Zone"
TASK_PROFILES["DZDI"] = "Danger Zone"

#====[WORKLOAD PROFILES]====
MAX_ASSET_PROFILES = {}
MAX_ASSET_PROFILES["W1"] = "1"
MAX_ASSET_PROFILES["W2"] = "2"
MAX_ASSET_PROFILES["W3"] = "3"
MAX_ASSET_PROFILES["W4"] = "4"
MAX_ASSET_PROFILES["W8"] = "8"
MAX_ASSET_PROFILES["W12"] = "12"
MAX_ASSET_PROFILE_INDEX = 2

#====[OPTIMAL INTERVENTIONS]====
OPTIMAL_INTERVENTIONS = {}
OPTIMAL_INTERVENTIONS["CC5"] = 5
OPTIMAL_INTERVENTIONS["NoCC"] = 0
OPTIMAL_INTERVENTIONS["CCD"] = 5
OPTIMAL_INTERVENTIONS["CCD5"] = 5
OPTIMAL_INTERVENTIONS["FCC5"] = 0
OPTIMAL_INTERVENTIONS["FCCD5"] = 5
OPTIMAL_INTERVENTIONS["FlatCC"] = 1
OPTIMAL_INTERVENTIONS["FlatNoCC"] = 0
OPTIMAL_INTERVENTIONS["OverflyCC"] = 1
OPTIMAL_INTERVENTIONS["FCC"] = 0

OPTIMAL_INTERVENTIONS["DZDI5"] = 5
OPTIMAL_INTERVENTIONS["DZNA5"] = 0
OPTIMAL_INTERVENTIONS["DZSI5"] = 0
OPTIMAL_INTERVENTIONS["DZUA5"] = 5

OPTIMAL_INTERVENTIONS["DZNA"] = 0
OPTIMAL_INTERVENTIONS["DZUA"] = 1
OPTIMAL_INTERVENTIONS["DZSI"] = 0
OPTIMAL_INTERVENTIONS["DZDI"] = 1

#====[BEHAVIOUR PROFILES]====
BEHAVIOUR_PROFILES = {}
BEHAVIOUR_PROFILES["CC5"] = "False Negative"
BEHAVIOUR_PROFILES["NoCC"] = "Errorless Active"#same thing
BEHAVIOUR_PROFILES["CCD"] = "Errorless Active"#same thing
BEHAVIOUR_PROFILES["CCD5"] = "Errorless Active"#same thing
BEHAVIOUR_PROFILES["FCC5"] = "Errorless Passive"
BEHAVIOUR_PROFILES["FCCD5"] = "False Positive"
BEHAVIOUR_PROFILES["FlatCC"] = "Collision Course - Flat"
BEHAVIOUR_PROFILES["FlatNoCC"] = "Errorless - Flat"
BEHAVIOUR_PROFILES["OverflyCC"] = "Collision Course - Overfly"
BEHAVIOUR_PROFILES["FCC"] = "Errorless - Overfly"

BEHAVIOUR_PROFILES["DZDI5"] = "False Negative"
BEHAVIOUR_PROFILES["DZNA5"] = "Errorless Active"
BEHAVIOUR_PROFILES["DZSI5"] = "Errorless Passive"
BEHAVIOUR_PROFILES["DZUA5"] = "False Positive"


BEHAVIOUR_PROFILES["DZNA"] = "Correct Diversion"
BEHAVIOUR_PROFILES["DZUA"] = "Incorrect Diversion"
BEHAVIOUR_PROFILES["DZSI"] = "Correct Incursion"
BEHAVIOUR_PROFILES["DZDI"] = "Incorrect Incursion"

BEHAVIOUR_PROFILES["CorrectDivert"] = "Correct Diversion"
BEHAVIOUR_PROFILES["IncorrectDivert"] = "Incorrect Diversion"
BEHAVIOUR_PROFILES["CorrectIncursion"] = "Correct Incursion"
BEHAVIOUR_PROFILES["IncorrectIncursion"] = "Incorrect Incursion"

#Other translation stuff :->
#====[GENDERS]]====
GENDERS = {}
GENDERS["1"] = "Male"
GENDERS["2"] = "Female"
GENDERS["3"] = "Non Binary"
GENDERS["4"] = "Other"


#Golbal System variables. No touch plis
gs_PARTICIPANT_COUNT = 0
gs_DEMOGRAPHICS_BUFFER = []

"""
datastructure:

particpant_results
    eventLog
    participantID
    surveyResults
    timeStamp
    trialResults
        behaviourSummary
            actions
            collisions
            divertCC
            divertCancelDZDI
            divertCancelDZSI
            divertDZDI
            divertDZSI
            divertFCC
            dzCrashes
            dzDangerousIncursions
            dzSafeIncursions
            interventions
            landings
            safeOverfly
        conditionString
        droneCount
        losses
        sceneName
        score
        transparency
        trialCode
"""

def getErrorProfileName(sceneName, delimiter, profileIndex):
    components = sceneName.split(delimiter)
    errorProfile = components[profileIndex]
    return errorProfile

def getErrorProfileNumbers(sceneName, delimiter, profileIndex):
    errorProfileName = getErrorProfileName(sceneName, delimiter, profileIndex)
    #CC	DZDI	DZUA	FCC	FCCD	DZSI	DZNA
    return ERROR_PROFILES[errorProfileName]

def getDisplayModeCode(sceneName, delimiter, displayModeIndex):    
    components = sceneName.split(delimiter)
    displayModeCode = components[displayModeIndex]
    return displayModeCode

def getDisplayModeString(sceneName, delimiter, displayModeIndex):
    displayModeCode = getDisplayModeCode(sceneName, delimiter, displayModeIndex)
    #print("getDisplayModeString: ", sceneName, ": ", displayModeCode)
    return DISPLAY_MODES[displayModeCode]    

def getTaskCode(sceneName, delimiter, taskModeIndex):    
    components = sceneName.split(delimiter)
    return components[taskModeIndex]    

def getTaskString(sceneName, delimiter, taskModeIndex):
    taskCode = getTaskCode(sceneName, delimiter, taskModeIndex)
    return TASK_PROFILES[taskCode]

def getAgentBehaviourString(sceneName, delimiter, taskModeIndex):
    taskCode = getTaskCode(sceneName, delimiter, taskModeIndex)
    return BEHAVIOUR_PROFILES[taskCode]
                              
def getMaxAssetProfile(sceneName, delimiter, taskModeIndex):
    components = sceneName.split(delimiter)
    return components[taskModeIndex]

def getMaxAssetCount(sceneName, delimiter, taskModeIndex):
    maxAssetProfile = getMaxAssetProfile(sceneName, delimiter, taskModeIndex)
    return MAX_ASSET_PROFILES[maxAssetProfile]

def translate_gender(gender):
    return GENDERS[str(gender)]

def translate_age(age):
    return str(17+age)
    
#get behaviour summary of a trial in multiple rows
def accumulateBehaviours(t):
    
    b = t["behaviourSummary"]
    interventions = []
    
    print("ZZZZZ")
    print(b)
    print("AAAAAA")
    print(str(b["divertCancelDZDI"]))
    print("GGGGGG")
    
    interventions.append("divertCC," + str(b["divertCC"]))
    interventions.append("divertCancelDZDI," + str(b["divertCancelDZDI"]))
    interventions.append("divertCancelDZSI," + str(b["divertCancelDZSI"]))
    interventions.append("divertDZDI," + str(b["divertDZDI"]))
    interventions.append("divertDZSI," + str(b["divertDZSI"]))
    interventions.append("divertFCC," + str(b["divertFCC"]))

    frontRowString = str(b["actions"]) + ","
    frontRowString += str(b["collisions"]) + ","

    backRowString = str(b["dzCrashes"]) + ","
    backRowString += str(b["dzDangerousIncursions"]) + ","
    backRowString += str(b["dzSafeIncursions"]) + ","
    backRowString += str(b["interventions"]) + ","
    backRowString += str(b["landings"]) + ","
    backRowString += str(b["safeOverfly"])

    accumulationRows = []

    for i in interventions:
        #print(i)
        accumulationRow = frontRowString
        accumulationRow += i + ","
        accumulationRow += backRowString
        accumulationRows.append(accumulationRow)

    return accumulationRows
    
#get behaviour summary of a trial in a single row
def flattenBehaviourSummary(b):
    rowString = str(b["actions"]) + ","
    rowString += str(b["collisions"]) + ","
    rowString += str(b["divertCC"]) + ","
    rowString += str(b["divertCancelDZDI"]) + ","
    rowString += str(b["divertCancelDZSI"]) + ","
    rowString += str(b["divertDZDI"]) + ","
    rowString += str(b["divertDZSI"]) + ","
    rowString += str(b["divertFCC"]) + ","
    rowString += str(b["dzCrashes"]) + ","
    rowString += str(b["dzDangerousIncursions"]) + ","
    rowString += str(b["dzSafeIncursions"]) + ","
    rowString += str(b["interventions"])
    return rowString

#flatten interaction log by summarising it
def summariseInteractionLog(iLog, optInt):
    interventions = 0
    correct = 0
    firstLog = None
    for i in iLog:
        if(firstLog == None):
            firstLog = i                    
        if("UserSaid-ALL_CLEAR" not in i["interactionType"]):
            interventions += 1
            
    reactionTime = firstLog["interactionTimestamp"]

    if(optInt == 0):
        if("UserSaid-ALL_CLEAR" in firstLog["interactionType"]):
           correct = 1
    else:
        if("yescancel" in firstLog["interactionTag"].lower()):
           correct = 1
        if("yesdivert" in firstLog["interactionTag"].lower()):
           correct = 1
        
    return [str(reactionTime) + "," + str(correct) , interventions]
    #return str(reactionTime) + ","

#flatten trial to a single row
# - flatMode: option flag for flatenned behaviour summary
def flattenTrial(t, flatMode):    
    sceneName = str(t["sceneName"])
    #print("SceneName: ", sceneName)
    displayModeName = getDisplayModeString(sceneName,'_',DISPLAY_MODE_INDEX)
    taskString = getTaskString(sceneName,'_',TASK_PROFILE_INDEX)
    maxAssetCount = getMaxAssetCount(sceneName,'_',MAX_ASSET_PROFILE_INDEX)
    optimalInterventions = OPTIMAL_INTERVENTIONS[getTaskCode(sceneName,'_',TASK_PROFILE_INDEX)]
    agentBehaviour = getAgentBehaviourString(sceneName,'_',TASK_PROFILE_INDEX)
    
    rowString = ""
    rowString += str(t["sceneName"]) + ","
    rowString += displayModeName + ","
    rowString += taskString + ","
    rowString += agentBehaviour + ","
    rowString += maxAssetCount + ", "
    rowString += str(optimalInterventions) + ","
    rowString += str(t["losses"]) + ","
    rowString += str(t["score"]) + ","
    rowString += str(t["trialCode"]) + ","
    rowString += str(t["conditionString"]) + ","
    rowString += str(t["transparency"]) + ","
    rowString += str(t["droneCount"]) + ","    

    "losses,score,trialCode,conditionString,transparency,droneCount"
    
    
    if flatMode:
        if "interactionLog" in t.keys():
            iLogSummary = summariseInteractionLog(t["interactionLog"], optimalInterventions)
            t["behaviourSummary"]["interventions"] = iLogSummary[1]
            rowString += flattenBehaviourSummary(t["behaviourSummary"]) + ","
            rowString += iLogSummary[0]

            #print("---- iLogSummmary:", iLogSummary)
        else:
            print("           Error in: " + sceneName + " - NO INTERACTION LOG!")
    return rowString
        
    

#flatten survey data to a single row
def flattenSurveys(surveys,trialIndex,surveysPerRow):
    outputRow = ""
    index = 0;

    #print("GGGGGGGGGGGGGGGG")
    #print(surveys[0]["answers"])

    taskDisplayFlag = False

    #print("flattenSurveys: ", trialIndex)
    
    for s in surveys:
        #print("flattenSurveys: ", index)
        if index >= (trialIndex * surveysPerRow) and index < ((trialIndex+1) * surveysPerRow):
            if("prevTrial" in s.keys() and not taskDisplayFlag and not s["prevTrial"] == ''):
                sceneName = s["prevTrial"]
                #print("flattenSurveys: prevTrial", sceneName)
                displayModeName = getDisplayModeString(sceneName,'_',DISPLAY_MODE_INDEX)
                taskString = getTaskString(sceneName,'_',TASK_PROFILE_INDEX)
                outputRow += sceneName + "," + displayModeName + "," + taskString + ","
                taskDisplayFlag = True


            aCount = 0
            for a in s["answers"]:                
                outputRow += str(a) + ","
                aCount += 1
            #print(" --------------- SURVEY: ", answersBuffer)
           
        index += 1

    outputRow = outputRow[:-2]#delete last comma before sending it back
    #print(" ----------- SURVEY OUTPUT ROW:", outputRow, "|")
    return outputRow

#parse the trials and surveys of a single participant
#  -flatMode: optionFlag for variables to be spread out accross more columns
def parse_participant_results(pr,sessionID,pi,flatMode):
    #print("SESSION->",sessionID)
    #2021-01-13-13-00-04_
    #2021-01-13-13-00-04_ (20)
    #X_1_2_2021-01-13-13-00-04_ (24)

    #split out participant id
    participantID = sessionID[20:]

    #leave out pilot sessions
    if len(participantID) < 4:
        return None
    

    #null pointer guard
    if (pr.get("trialResults") is None ):
        return None
    
    trialRows = []
    surveyBlockRows = []

    #getting rid of practice rows
    allTrialRows = (pr["trialResults"])
    practiceCount = count_practice_runs(allTrialRows)

    #When in 1337 mode, ignore noobs that need more than 1 run of the practice
    if(L337_MODE):
        if(practiceCount > 4):
            return None


    relevantTrials = allTrialRows[practiceCount:]
    
    trialCount = len(relevantTrials)
    if trialCount != VALID_TRIAL_COUNT:
        print("!!!!!!!!!     INVALID TRIAL COUNT    !!!!!!!!!")
        print(participantID, trialCount)
        print(".")
        print(".")
        return None
    
    #extract survey data
    surveyRows = (pr["surveyResults"])
    gender = translate_gender(surveyRows[1]['answers'][0])
    age = translate_age(surveyRows[2]['answers'][0])
    demographics = sessionID + "," + participantID + "," + str(pi) + "," + str(gender) + "," + str(age) + ","

    #isolate inter-trial surveys
    trialSurveys = surveyRows[TRIAL_SURVEY_START_INDEX:]
    trialSurveys = list(filter(None,trialSurveys))#dump empty surveys

    global gs_DEMOGRAPHICS_BUFFER    

    gaming_q1 = surveyRows[0]['answers'][0]
    gaming_q2 = surveyRows[0]['answers'][1]
    gaming_q3 = surveyRows[0]['answers'][2]

    gaming_surveys = str(gaming_q1) + "," + str(gaming_q2) + "," + str(gaming_q3)
    
    gs_DEMOGRAPHICS_BUFFER.append(demographics + gaming_surveys)

    #print("-----")
    #print("-----")
    #print("-----")


    #collate trial data
    index = 0
    for rt in relevantTrials:
        if FLAT_MODE:            
            trialRow = demographics + str(index + 1) + "," + flattenTrial(rt, True)
            trialRows.append(trialRow)
            
        else:            
            frontRowString = sessionID + "," + participantID + "," + str(gender) + "," + str(age) + "," + flattenTrial(rt, False)
            backRowString = ","+ flattenSurveys(trialSurveys,index,SURVEYS_PER_BLOCK)
            accumulatedBehaviours = accumulateBehaviours(rt)

            for splitBehaviours in accumulatedBehaviours:
                trialRow = frontRowString + splitBehaviours + backRowString
                trialRows.append(trialRow)

        index += 1
            
    #collate survey data
    for index in range(SURVEY_BLOCKS):
        #print("---Collate survey data---");
        surveyBlockRow = demographics + flattenSurveys(trialSurveys,index,SURVEYS_PER_BLOCK)
        surveyBlockRows.append(surveyBlockRow)

    #keep track of parsed participants
    global gs_PARTICIPANT_COUNT
    gs_PARTICIPANT_COUNT += 1
    
    return [trialRows,surveyBlockRows]

def count_practice_runs(trialRows):
    count = 0

    for row in trialRows:
        sceneName = row["sceneName"]        
        if("practice" in sceneName.lower()):
           count += 1

    return count
    

#
#=-=-=-=-=-=[FILE WRITING]=-=-=-=-=-=
#

csvBuffer = ""
surveyBuffer = ""
demographicsBuffer = ""
newLine = "\n"

#main data
participantOrder = 1
for participant_results in data:
    print(participant_results)
    outputRows = parse_participant_results(data[participant_results],participant_results,participantOrder,FLAT_MODE)

    if(outputRows != None):
        participantOrder += 1
        for r in outputRows[0]:        
                csvBuffer += r + newLine
        for r in outputRows[1]:        
                surveyBuffer += r + newLine
                
#demographics summary
for d in gs_DEMOGRAPHICS_BUFFER:
    demographicsBuffer += d + newLine
    
demographicsHeader = "sessionID,participantID,participantIndex,gender,age"
gamingSurveyHeader = ",gaming_q1, gaming_q2, gaming_q3"
demographicsBuffer = demographicsHeader + gamingSurveyHeader + newLine + demographicsBuffer


#participantDetailHeaders = "sessionID,participantID,participantIndex,gender,age,"
participantDetailHeaders = demographicsHeader + ","
#Trial CSV Row Headers
flatRowHeaders = participantDetailHeaders
flatRowHeaders += "trialOrder,sceneName,displayMode,task,agentBehaviour,maxDroneCount,optimalInterventions,losses,score,trialCode,conditionString,transparency,droneCount,"
flatRowHeaders += "actions,collisions,divertCC,divertCancelDZDI,divertCancelDZSI,divertDZDI,divertDZSI,divertFCC,dzCrashes,dzDangerousIncursions,dzSafeIncursions,interventions,"
flatRowHeaders += "fInt_rTime,fInt_isCorrect" + newLine
#flatRowHeaders += "CAP_1,CAP_2,CAP_3,TLX_1,TLX_2,TLX_3,TLX_4,TLX_5,TLX_6,OFlow_1,OFlow_2,OFlow_3,OFlow_5,OFlow_6"+ newLine

#the tall version collapses divertCC and other interventions into two columns: "interventionType" and "interventionCount"
tallRowHeaders = participantDetailHeaders
tallRowHeaders += "conditionString,droneCount,losses,sceneName,score,transparency,trialCode,actions,collisions,interventionType,interventionCount,dzCrashes,dzDangerousIncursions,dzSafeIncursions,interventions,landings,safeOverfly,"
#tallRowHeaders += "CAP_1,CAP_2,CAP_3,TLX_1,TLX_2,TLX_3,TLX_4,TLX_5,TLX_6,OFlow_1,OFlow_2,OFlow_3,OFlow_5,OFlow_6"+ newLine

#Trial CSV header installation
if FLAT_MODE:
    csvBuffer = flatRowHeaders + csvBuffer
else:
    csvBuffer = tallRowHeaders + csvBuffer

#Survey CSV Row Headers
surveyRowHeaders = participantDetailHeaders
surveyRowHeaders += "blockFinalTrial,displayMode,task,"
#surveyRowHeaders += "CAP_1,CAP_2,CAP_3,CAP_4,CAP_5,CAP_6,"
surveyRowHeaders += "TLX_1,TLX_2,TLX_3,TLX_4,TLX_5,TLX_6,MV_1,MV_2,MV_3,MV_4" + newLine
surveyBuffer = surveyRowHeaders + surveyBuffer

print()
print()
print("ELITE MODE: ", str(L337_MODE))
print("PARTICIPANTS PROCESSED: ", str(gs_PARTICIPANT_COUNT))
#print("RESULTING CSV:")
#print(csvBuffer)

#WRITE THE FILLEESS!!
modePrefix = "TALL"
if(FLAT_MODE):
    modePrefix = "FLAT"

outputCSV = open(path+"\\"+modePrefix + "_" + inputJSON + outputSuffix + ".csv", "w")
outputCSV.write(csvBuffer)
outputCSV.close()

outputSurveyCSV = open(path+"\\SURVEY_" + inputJSON + outputSuffix + ".csv", "w")
outputSurveyCSV.write(surveyBuffer);
outputSurveyCSV.close()

outputDemographicsCSV = open(path+"\\DEMOGRAPHICS_" + inputJSON + outputSuffix + ".csv", "w")
outputDemographicsCSV.write(demographicsBuffer);
outputDemographicsCSV.close()
