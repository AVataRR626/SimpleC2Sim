"""
by Matt Cabanag, 2024

little helper code for querying the participant data

main() writes accuracy CSV

"""


import os
import csv

inputCSV = "FLAT_c2sim-DZ.json.csv"
outputFName = inputCSV + "_ACCRBEHAV_AgBCombined.csv"
path = os.getcwd()

class Trial:
    def __init__(self, sessionID, trialSeq):        
        self.sessionID = sessionID
        self.trialSeq = trialSeq

    def SetDisplayMode(self, displayMode):
        self.displayMode = displayMode

    def SetWorkload(self, workload):
        self.workload = workload

    def SetTask(self, task):
        self.task = task

    def SetReactionTime(self, reactionTime):
        self.reactionTime = reactionTime

    def SetIsCorrect(self,isCorrect):
        self.isCorrect = isCorrect

    def SetOptimalInterventions(self, optimalInterventions):
        self.optimalInterventions = optimalInterventions

    def SetInterventions(self, interventions):
        self.interventions = interventions

    def SetAccuracy(self, accuracy):
        self.accuracy = accuracy


class Participant:
    def __init__(self, pid, index):
        self.pid = pid
        self.index = index
        self.trials = []

    def AddTrial(self, trial_obj):
        self.trials.append(trial_obj)

    def GetAccuracy(self, task, workload, displayMode):
        isCorrectTotal = 0
        totalCount = 0
        #print("Average for: ", task, " and ", workload, " and ", displayMode)        
        for t in self.trials:            
            if t.task == task and t.workload == workload and t.displayMode == displayMode:
                totalCount += 1                
                if int(t.isCorrect) == 1:
                    isCorrectTotal += 1

        #print(isCorrectTotal, totalCount)
        return isCorrectTotal/totalCount

    def GetReactionTime(self, task, workload, displayMode):
        count = 0
        timeTotal = 0

        for t in self.trials:
            if t.task == task and t.workload == workload and t.displayMode == displayMode:
                count += 1
                timeTotal += float(t.reactionTime)

        return float(timeTotal)/count

if __name__ == '__main__':
    file = open(path+"\\"+inputCSV)
    lines = file.readlines()
    count = 0

    prev_pid = "START"
    participantList = []

    #ingest all trials per participant
    for line in lines:
        components = line.split(",")        
        current_pid = components[1]

        if(prev_pid != current_pid):
            participantList.append(Participant(current_pid, components[2]))

        newTrial = Trial(components[0], components[5])
        newTrial.SetDisplayMode(components[7])
        newTrial.SetTask(components[8])
        newTrial.SetOptimalInterventions(components[11])
        newTrial.SetInterventions(components[29])                                 
        newTrial.SetWorkload(components[17])
        newTrial.SetInterventions(components[29])
        newTrial.SetReactionTime(components[30])
        newTrial.SetIsCorrect(components[31])
        

        participantList[(len(participantList)-1)].AddTrial(newTrial)

        prev_pid = current_pid
        count += 1

    #cut off the row headings
    participantList = participantList[1:]

    newLine = "\n"
    rowHeaders = "participantID,participantIndex,task,maxDroneCount,displayMode,accuracy,reactionTime,a2rt" + newLine
    
    outputCSV = open(path + "\\" + outputFName, "w")
    csvBuffer = rowHeaders


    for p in participantList:
        participantInfo = p.pid + "," + p.index + ", " + "Danger Zone," 
        for w in ("1","2","3"):
        #for w in ("4","8","12"):
            workload = w + ","
            for d in ("Text","Graphical", "Text+Graphical"):
            #for d in ("Text","Graphical"):                
                displayMode = d + ","
                #print(d, w, participantInfo)
                a = p.GetAccuracy("Danger Zone", w, d)
                rt = p.GetReactionTime("Danger Zone", w, d)
                accuracy = str(a) + ","
                reactionTime = str(rt) + ","
                a2rt = str(a/rt)
                csvBuffer += participantInfo + workload + displayMode + accuracy + reactionTime + a2rt + newLine
                

    print("Total Lines Read: " + str(count))
    print("Participant List Size: " + str(len(participantList)))
    outputCSV.write(csvBuffer)
    outputCSV.close()

