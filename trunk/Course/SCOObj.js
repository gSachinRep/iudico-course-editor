function SCOObj(passRank)
{
  this.passRank = passRank;
  this.tests = new Array();
  this.compiled = 0;
  
  this.length = function()
  {
    return this.tests.length;
  }
  
  this.Commit = function()
  {
	doInitialize();
	
    for (var i = 0 ; i < this.length(); i++)
    {
		if (this.tests[i].CompiledTest = true)
		{
			this.tests[i].getAnswer(this, i);
			this.compiled++;
		}
		else
		{
			doSetValue("cmi.interactions." + i + ".learner_response", this.tests[i].getAnswer());
		}
    }
    
	$('#Button1').disabled = true;

	if (this.compiled == 0)
	{
		this.FinishUp();
	}
  }
  
  this.FinishUp = function()
  {
	this.compiled--;
	
	if (this.compiled <= 0)
	{
		doSetValue("cmi.completion_status", "completed");
		doSetValue("cmi.exit", "suspend");
		  
		doCommit();
		doTerminate();
	}
  }
  
  var argLength = arguments.length;

  for (var i = 1; i < argLength; i++)
  {
    this.tests.push(arguments[i]);
    
    numInteractions = doGetValue("cmi.interactions._count");
    //alert(numInteractions);
    if (numInteractions == 0)
    {
      doSetValue("cmi.interactions." + (i-1) + ".correct_responses.0.pattern", this.tests[i-1].getCorrectAnswer());
      doSetValue("cmi.interactions." + (i-1) + ".type", this.tests[i-1].getType());
    }
    else if (numInteractions > 0)
    {
      learnerResponse = doGetValue("cmi.interactions." + (i-1) + ".learner_response");
      //alert(learnerResponse);
      if (learnerResponse)
      {
        $('#Button1').disabled = true;
        this.tests[i-1].setAnswer(learnerResponse);
        //doGetValue("cmi.interactions." + (i-1) + ".correct_responses.0.pattern");
      }
    }
  }
}

function simpleTest(ID, correctAnswer)
{
  this.ID = ID;
  this.correctAnswer = correctAnswer;
	
  this.getAnswer = function()
  {
    return $('#' + this.ID).value;
  }
  
  this.setAnswer = function(answer)
  {
    document.$('#' + this.ID).value = answer;
  }
  
  this.getCorrectAnswer = function()
  {
    return this.correctAnswer;
  }
  
  this.getType = function()
  {
    return "fill-in";
  }
}

function complexTest(ID, correctAnswer)
{
  this.ID = ID;
  this.correctAnswer = correctAnswer;
  
  this.getAnswer = function()
  {
    var result = [];
    var inputArray = $('#' + this.ID + ' input');
    
    for(var i=0; i<inputArray.length; i++)
    {
      result[i] = (inputArray[i].checked ? "1" : "0");
    }
		
    return result.join(',');
  }
  
  this.setAnswer = function(answer)
  {
    var result = answer.split(',');
    var inputArray = $('#' + this.ID + ' input');
    
    for (var i = 0; i < inputArray.length; i++)
    {
      inputArray[i].checked = (result[i] == "1");
    }
  }
  
  this.getCorrectAnswer = function()
  {
    return this.correctAnswer;
  }
  
  this.getType = function()
  {
    return "choice";
  }
}

function compiledTest(IDBefore, IDAfter, ID, url, language, timelimit, memorylimit, input, output)
{
	this.CompiledTest = true;
	
	this.ID = ID;
	this.IDBefore = IDBefore;
	this.IDAfter = IDAfter;
	this.language = language;
	this.timilimit = timelimit;
	this.memorylimit = memorylimit;
	this.input = input;
	this.output = output;
	this.url = url;
	
	jQuery.flXHRproxy.registerOptions(this.url, {xmlResponseText: false, loadPolicyURL: 'http://localhost:49440/crossdomain.xml', onerror: this.handlError});
	
	this.getAnswer = function(SCOObj, i)
	{
		var sourceT = $('#' + this.IDBefore + ' pre').text() + $('#' + this.ID).val() + $('#' + this.IDAfter + ' pre').text();
		var dataT = {'source': sourceT, 'language': language, 'input': input, 'output': output, 'timelimit': timelimit, 'memorylimit': memorylimit};

		$.ajax({
			type: "POST",
			url: this.url,
			data: dataT,
			dataType: 'xml',
			transport: 'flXHRproxy',
			complete: function(transport)
			{
				var bool = ($(transport.responseText).text());
				doSetValue("cmi.interactions." + i + ".learner_response", bool);
				SCOObj.FinishUp();
			}
		});
	}
	
	this.setAnswer = function(answer)
	{
		$('#' + this.ID).value = answer;
	}
	
	this.getCorrectAnswer = function()
	{
		return "true";
	}
	
	this.getType = function()
	{
		return "true-false";
	}
}