#version 330 core
layout (location = 0) out vec4 FragColor;

uniform float time;
uniform vec3 camPosition;
uniform mat4 view;


in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
} fs_in;

#define MAX_STEPS 100
#define MAX_DISTANCE 100.
#define DISTANCE_THRESH .01


float getDistMandelbulb(vec3 pos) {
    float Power = 10 * sin(time/10);
	vec3 z = pos;
	float dr = 1.0;
	float r = 0.0;
	for (int i = 0; i < 20 ; i++) {
		r = length(z);
		if (r>2) break;
		
		// convert to polar coordinates
		float theta = acos(z.z/r);
		float phi = atan(z.y,z.x);
		dr =  pow( r, Power-1.0)*Power*dr + 1.0;
		
		// scale and rotate the point
		float zr = pow( r,Power);
		theta = theta*Power;
		phi = phi*Power;
		
		// convert back to cartesian coordinates
		z = zr*vec3(sin(theta)*cos(phi), sin(phi)*sin(theta), cos(theta));
		z+=pos;
	}
	return 0.5*log(r)*r/dr;
}

float getDistManySpheres(vec3 p, float r) {
    p= mod((p),time/10)-0.5;
    return length(p)-r;
}

float getDistCube(vec3 p, vec3 c, vec3 size) {
    return length(max(abs(p-c)-size, 0));
}

float getDistSphere(vec3 p, vec3 c, float r) {
    return length(p-c)-r;
}

float getDistTorus(vec3 p, vec3 c, vec2 r) {
    float x = length((p-c).xz) - r.x;
    return length(vec2(x, (p-c).y)) - r.y;
}

float getDistCapsule(vec3 p, vec3 a, vec3 b, float r) {
    vec3 ab = b-a;
    vec3 ap = p-a;
    float t = dot(ap,ab)/dot(ab,ab);
    t=clamp(t,0,1);
    vec3 c = a + t*ab;

    return length(c-p) - r;
}

//Get distanse from point p to the scene.
float getDist(vec3 p) {
    //float sphereDist = getDistSphere(p, vec3(0, 1 , 6), 1.);
    //float capsDist = getDistCapsule(p, vec3(0, 1 , 6), vec3(1, 2 , 6), .5);
    //float torusDist = getDistTorus(p, vec3(-.3, .5 , 6),vec2(1.5, .5));
    //float cubeDist = getDistCube(p, vec3(-1, 1 , 6),vec3(.3, .5 , .6));
    //float sphereDist = getDistManySpheres(p - vec3(0, 1, 0), 0.1);
    float mandelbulbDist = getDistMandelbulb((p - vec3(0, 1 , 3)));
    //float planeDist = p.y;
    return mandelbulbDist;
}


float rayMarch(vec3 ro, vec3 rd) {
    float dO = 0;
    for(int i = 0; i< MAX_STEPS; i++){
        vec3 p = ro + rd*dO;
        float ds = getDist(p);
        dO += ds;
        if (dO > MAX_DISTANCE || ds < DISTANCE_THRESH) {
            break;
        }
    }
    return dO;
}

vec3 getNormal(vec3 p) {
    float d = getDist(p);
    vec2 e = vec2(.01, 0);
    vec3 n = d - vec3(
        getDist(p - e.xyy),
        getDist(p - e.yxy),
        getDist(p - e.yyx)
    );
    return normalize(n);
}

float shadow( in vec3 ro, in vec3 rd, float k)
{
    float res = 1.0;
    for( float t=0; t<MAX_STEPS; )
    {
        float h = getDist(ro + rd*t);
        if( h<0.001 )
            return 0.0;
        res = min( res, k*h/t );
        t += h;
    }
    return res;
}

float getLight(vec3 p) {
    vec3 lightPos = vec3(0,5,2);
    lightPos.xz += vec2(sin(time), cos(time));
    vec3 l = normalize(lightPos - p);
    vec3 n = getNormal(p);

    float dif = clamp(dot(n,l), 0, 1);
    return dif;
}



void main()
{   
    vec2 uv = (fs_in.TexCoords - .5);

    // position of the camera.
    vec3 ro = camPosition;
    // ray direction.
    vec3 rd = mat3(view)*normalize(vec3(uv.xy,1));
    // Distance to the intersection with the scene.
    float d = rayMarch(ro, rd);
    // Point of intersection
    vec3 p = ro + rd * d;

    float dif = getLight(p);
    

    vec3 col = vec3(dif);

    FragColor = vec4(col,1.0);
}