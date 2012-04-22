var target : Transform;
var distance = 1.25;
var xSpeed = 250.0;
var ySpeed = 120.0;
var yMinLimit = -20;
var yMaxLimit = 80;
var zoomRate = 25;

private var x = 0.0;
private var y = 0.0;

@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
}
	
function Update () {
	if (target) {
       
    }
	

}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}