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
    for (var i = 0 ; i < this.length(); i++)
    {
		if (this.tests[i].CompiledTest == true)
		{
			this.tests[i].processAnswer(this, i);
			this.compiled++;
		}
		else
		{
			doSetValue("cmi.interactions." + i + ".learner_response", this.tests[i].getAnswer());
			doSetValue("cmi.interactions." + i + ".result", this.tests[i].getResult());
		}
    }
    
	$('#Button1')[0].disabled = true;

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
		doSetValue("cmi.exit", "normal");
	  
		doCommit();
    doSetValue("adl.nav.request", "continue");
		doTerminate();
	}
  }
  
  doInitialize();
  
  var argLength = arguments.length;

  for (var i = 1; i < argLength; i++)
  {
    this.tests.push(arguments[i]);
    
    numInteractions = doGetValue("cmi.interactions._count");
    //alert(numInteractions);
    if (numInteractions == 0)
    {
      doSetValue("cmi.interactions." + (i-1) + ".id", this.tests[i-1].ID);
      doSetValue("cmi.interactions." + (i-1) + ".type", this.tests[i-1].getType());      
      doSetValue("cmi.interactions." + (i-1) + ".correct_responses.0.pattern", this.tests[i-1].getCorrectAnswer());      
    }
    else if (numInteractions > 0)
    {
      learnerResponse = doGetValue("cmi.interactions." + (i-1) + ".learner_response");
      //alert(learnerResponse);
      if (learnerResponse)
      {
        $('#Button1')[0].disabled = true;
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
    return $('#' + this.ID)[0].value;
  }
  
  this.setAnswer = function(answer)
  {
    document.$('#' + this.ID)[0].value = answer;
  }
  
  this.getCorrectAnswer = function()
  {
    return this.correctAnswer;
  }
  
  this.getType = function()
  {
    return "fill-in";
  }
  
  this.getResult = function()
  {
	return (this.getCorrectAnswer() == this.getAnswer() ? "correct" : "incorrect");
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
		
    return result.join('');
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
  
  this.getResult = function()
  {
	return (this.getCorrectAnswer() == this.getAnswer() ? "correct" : "incorrect");
  }
}

function compiledTest(IDBefore, IDAfter, ID, url, language, timelimit, memorylimit, input, output)
{
	this.CompiledTest = true;
	
	this.ID = ID;
	this.IDBefore = IDBefore;
	this.IDAfter = IDAfter;
	this.language = language;
	this.timelimit = timelimit;
	this.memorylimit = memorylimit;
	this.input = input;
	this.output = output;
	this.url = url;
	
	jQuery.flXHRproxy.registerOptions(this.url, {xmlResponseText: false});
	
	this.processAnswer = function(SCOObj, i)
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
				var response = ($(transport.responseText).text());
				doSetValue("cmi.interactions." + i + ".learner_response", response);
				doSetValue("cmi.interactions." + i + ".result", (response == "Accepted" ? "correct" : "incorrect"));
				SCOObj.FinishUp();
			}
		});
	}
	
	this.setAnswer = function(answer)
	{
		$('#' + this.ID).value = answer;
	}
	
	this.getAnswer = function()
	{
		return "";
	}
	
	this.getCorrectAnswer = function()
	{
		return "Accepted";
	}
	
	this.getType = function()
	{
		return "other";
	}
}

