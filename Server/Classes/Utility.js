module.exports =  {
    Comma: function(data = String){
        return parseFloat(data.replace(",",".")); 
    },
    Period: function(data = Number){
        return data.toString().replace(".",",");
    }
}