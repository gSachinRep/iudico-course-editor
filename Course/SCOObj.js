function SCOObj(passRank)
{
    this.passRank = passRank;
    this.tests = new Array();
    this.length = function(){
        return this.tests.length;
    }
    this.Commit = function(){
        doInitialize();
        for(var i=0;i<this.length();i++)
            doSetValue("cmi.answers." + i + ".value", this.tests[i].getAnswer());
        doSetValue("cmi.completion_status", "completed");
        doSetValue("cmi.exit", "normal");
        doCommit();
        doTerminate();
    }
    
    var c=arguments.length;
    for (var i=1; i<c; i++)
        this.tests.push(arguments[i]);
}

function simpleTest(ID){
    this.ID = ID;
    this.getAnswer = function(){
        return document.getElementById(this.ID).value;
    }
}

function complexTest(ID){
    this.ID = ID;
    this.getAnswer = function(){
        var result = "";
        var inputArray = document.getElementById(this.ID).getElementsByTagName("input");
        for(var i=0; i<inputArray.length; i++)
            result+=inputArray[i].checked ? "1":"0";
        return result;
    }
}