function traceLog(message){
    var tl = document.getElementById('traceLog');
    tl.style.display = 'inline';
    tl.value = (new Date()).toLocaleTimeString() + ': ' + message + '\n' + tl.value;
}

function LMSDebugger(){
    this.Initialize = function(parameter){
        traceLog('Initialize(\'' + parameter + '\')');
        return "true";
    }
    
    this.Terminate = function(parameter){
        traceLog('Terminate(\'' + parameter + '\')');
        return "true";
    }
    
    this.GetValue = function(name){
        traceLog('GetValue(\'' + name + '\')');
        return 0;
    }
    
    this.SetValue = function(name, value){
        traceLog('SetValue(\'' + name + '\', \'' + value + '\')');
    }
    
    this.Commit = function(parameter){
        traceLog('Commit(\'' + parameter + '\')');
    }
    
    this.GetLastError = function(){
        return 0;
    }
    
    this.GetErrorString = function(parameter){
        return "No error ocurred";
    }
    
    this.GetDiagnostic = function(parameter){
        return "No diagnostic information available";
    }
}