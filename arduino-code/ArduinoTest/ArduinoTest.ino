
const int buttonPin = 2;
const int ledPin =  13;


int buttonState = 0;


String inputString = ""; 
boolean stringComplete = false;


void setup() 
{
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);      
  pinMode(buttonPin, INPUT);     
}

void loop()
{
  button();
  led();
}


void button()
{
  int newButtonState = digitalRead(buttonPin);
  
  if(buttonState != newButtonState)
  {
    if (newButtonState == HIGH) 
    {    
      Serial.println("b_up");
    } 
    else 
    {
      Serial.println("b_down"); 
    }
    buttonState = newButtonState;
  }
}


void led()
{
  if (stringComplete) 
  {
    if(inputString=="l_on")
    {
      digitalWrite(ledPin, HIGH);
    }
    else
    {
      digitalWrite(ledPin, LOW);
    }
    
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
}


void serialEvent() 
{
  while (Serial.available()) 
  {
    char inChar = (char)Serial.read(); 
    inputString += inChar;
    if (inChar == '\n') 
    {
      stringComplete = true;
    } 
  }
}
