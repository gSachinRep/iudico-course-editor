function SCOObj(passRank)
{
    this.passRank = passRank;
    this.tests = new Array();
    this.length = function(){
        return this.tests.length;
    }
    this.Commit = function(){
        doInitialize();
        
        var score = 0;
        
        for(var i=0;i < this.length();i++) {
            score += (this.tests[i].getScore() * this.tests[i].Rank );
        }
        //alert(score);
        doSetValue("cmi.score", (( score >= passRank) ? 1 : 0)); 
          
        //doSetValue("cmi.answers." + i + ".value", this.tests[i].getAnswer());
        doSetValue("cmi.completion_status", "completed");
        doSetValue("cmi.exit", "normal");
        doCommit();
        doTerminate();
    }
    
    var c = arguments.length;
    for (var i=1; i<c; i++)
        this.tests.push(arguments[i]);
}

function simpleTest(ID, rank, answer){
    this.ID = ID;
    this.Rank = rank;
    this.Answer = answer;
    
    this.getAnswer = function(){
        return document.getElementById(this.ID).value;
    }
    
    this.getScore = function(){
        return (this.getAnswer() == this.Answer) ? 1 : 0;
    }
}

function compiledTest(ID, rank, memoryLimit, timeLimit, outputLimit, language, address){
    this.ID = ID;
    this.Rank = rank;
    this.MemoryLimit = memoryLimit;
    this.TimeLimit = timeLimit;
    this.OutputLimit = outputLimit;
    this.Language = language;
    this.Address = address;
    this.TestCases = new Array();
    
    this.getAnswer = function() {
        return document.getElementById(this.ID).value;
    }
    this.getScore = function() {
        var res = service(
                this.getAnswer(), 
                new this.allInfo(
                    this.Rank, this.MemoryLimit, this.TimeLimit, this.OutputLimit, this.Language, this.TestCases
                    ),
                this.Address
            );
        
        return res;
    }
    
    this.allInfo = function(rank, memoryLimit, timeLimit, outputLimit, language, testCases) {
    
        this.Rank = rank;
        this.MemoryLimit = memoryLimit;
        this.TimeLimit = timeLimit;
        this.OutputLimit = outputLimit;
        this.TestCases = testCases;
        this.Language = language;
    }
    
    var c = arguments.length;
    for (var i = 6; i < c; i++) {
        this.TestCases.push(arguments[i]);
    }
}
function TestCase(input, output) {
    this.Input = input;
    this.Output = input;
}
function service(userAnswer, allInfo, serviceAddress)
{
    var input =new Array();
    var output = new Array();
    for(var i = 0; i < allInfo.TestCases.length; i++)
    {
        input[i] = allInfo.TestCases[i].Input;
        output[i]= allInfo.TestCases[i].Output;

    }
    $.ajax({
        type: "POST",
        url: serviceAddress, //webservice url
        contentType: "application/x-www-form-urlencoded",
        data:{'source': userAnswer, 'language': allInfo.Language,'input': input , 'output': output,'timeLimit': allInfo.TimeLimit,'memoryLimit':allInfo.MemoryLimit},
        async: false,
        success: 
            function(response)
            {
               var x=response.getElementsByTagName("float")[0];
               var y=x.childNodes[0];
               return y.nodeValue; 
            },
        error: 
            function(response)
            {
                return 0;
            }
    });
    
}

function complexTest(ID, rank, answer){
    this.ID = ID;
    this.Rank = rank;
    this.Answer = answer;
    
    this.getAnswer = function(){
        var result = "";
        var inputArray = document.getElementById(this.ID).getElementsByTagName("input");
        for(var i=0; i<inputArray.length; i++)
            result+=inputArray[i].checked ? "1":"0";
        return result;
    }
    
    this.getScore = function() {
        return (this.getAnswer() == this.Answer) ? 1 : 0;;
    }
}