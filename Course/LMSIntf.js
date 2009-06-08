var findAPITries;
var apiHandle = null;
var exitPageStatus = false;

function isEmulationMode(){
    return window.location.search.toString().toLowerCase().indexOf('emulatelms=true') >= 0;
}

function findAPI(win){
   while (win.API_1484_11 == null && win.parent != null && win.parent != win)
      if (findAPITries++ > 500)
         return null;
      else
        win = win.parent;
   return win.API_1484_11;
}

function getAPI(){
    var result = null;
    if (isEmulationMode())
        result = new LMSDebugger();
    else{
       findAPITries = 0;
       result = findAPI(window);

       if (apiHandle == null && window.opener != null && typeof(window.opener) != "undefined")
          result = findAPI(window.opener);
    }
    return result;
}

function getAPIHandle(){
    if (apiHandle == null)
            apiHandle = getAPI();

   return apiHandle;
}

function checkForError(){
    var errCode = getAPIHandle().GetLastError();
    if (errCode != 0)
        alert(getAPIHandle().GetErrorString(errCode).toString());
}

var inited = false;

function doTerminate(){
    if (inited){
        inited = false;
        getAPIHandle().Terminate("");
        exitPageStatus = true;
        checkForError();
    }
}

function doInitialize(){
    if (!inited){
        getAPIHandle().Initialize("");
        checkForError();
        inited = true;
    }
}

function doGetValue(name){
    var value = getAPIHandle().GetValue(name);
    checkForError();
    return value;
}

function doSetValue(name, value){
   getAPIHandle().SetValue(name, value);
   checkForError();
}

function doCommit(){
    var result = getAPIHandle().Commit("");
    checkForError();
    return result;
}

function getObjectiveIndex(id){
    var count = doGetValue("cmi.objectives._count");
    for (i = 0; i < count; i++)
        if (doGetValue("cmi.objectives." + i + ".id") == id)
            return i;
    return -1;
}

function getScore(objectiveID){
    var index = getObjectiveIndex(objectiveID);
    var res = doGetValue("cmi.objectives." + index + ".score.scaled");
    return res != null ? res : 0;
}

var totalMax = 0;
var total = 0;
function resetPoints()
{
    total = 0;
    totalMax = 0;
}

function getTotalMessage()
{
    return 'Total: ' + total + ' / ' + totalMax; // Потенційні граблі!!!
}

function updateSummaryPageItem(id, maxPoints, passPoints)
{
    var item = document.getElementById(id);
    var points = getScore(id);
    if (points != null)
    {
        total = total + points;                   // Потенційні граблі!!!
    }
    if (maxPoints != null)
    {
        totalMax = totalMax + maxPoints;
    }
    item.innerHTML = points + ' / ' + maxPoints;
    if (passPoints != null && points < passPoints)
    {
        item.parentNode.style.color = 'red';
    }
}