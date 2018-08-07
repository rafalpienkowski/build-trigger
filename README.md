# Build trigger

## Purpose of the project
___

This project has been made to allow multiple users to trigger a build defined in Visual Studio Team Services (VSTS). Due to some [limitation of a free version of VSTS](https://visualstudio.microsoft.com/team-services/pricing/), there was a need to enable build trigger to the project contributors (which don't have such permission). Contributors don't have to have permission to create and modify build definition. The solution for our problem was to generate a Personal Access Token (PAT) in VSTS, build a small application which will be deployed on a local server and give anyone in the team ability to trigger a build.

Project search for avaliable build definiotions which have been setup in the VSTS project. Avaliable permission are visible for everyone. To trigger a bulild just click on it's name.

_Notice:_ All build will be trigger in context on PAT token issuer.

![Dashboard](https://raw.githubusercontent.com/rafalpienkowski/resources/master/build-trigger/dashboard.png)

## Setup
___

Remember to setup VstsBuildOptions in the BuildTrigger.Web project

```
  "VstsBuildOptions": {
    "Instance": "instance",
    "Project": "project",
    "TokenBase64": "tokenBase64"
  } 
```

Parameters:
- Instance

    Instance name is the name of your organization. It's a part of the VSTS url. Example below:
    ``` 
    https://{instance}.visualstudio.com/
    ```

- Project

    Name of project which contains build definitions. Project's name is also a part of VSTS url. The Example below: 
    ```
    https://{instance}.visualstudio.com/{project}
    ```

- TokenBase64

  This is a base64 encoded a combination of username and Personal Access Token (PAT) which has been created for given user. 
  
    To create such a token you need to perform base64 encoding on a string containing user_name:pat_token like on the example below. _Notice_: remember to add ":" between user_name and pat_token.

    ```
    base64encode(user_name:pat_token)
    ```
  
  More about PAT you can find in [official documentation](https://docs.microsoft.com/en-us/vsts/organizations/accounts/use-personal-access-tokens-to-authenticate?view=vsts). 


## Docker
___

There is also avaliable docker image on [DockerHub](https://hub.docker.com/r/rafalpienkowski/build-trigger/) if you don't like to run the project on local machine.

Example usage which start a container and maps port 80 from container to port 5000 on the host machine:

```
 docker run -p 5000:80 -e "VstsBuildOptions:Instance={instance}" -e "VstsBuildOptions:Project={project}" -e "VstsBuildOptions:TokenBase64={tokenBase64}" rafalpienkowski/build-trigger
```

_Notice:_ Please remember to setup **project, instance and token**. In other case there will be no build definition downloaded from VSTS.

## Other
___

This project is under the MIT Licence. Feel free to copy it and modify it. If you have any proposition for new features, give me info or create a Pull Request :wink:. It will be a great pleasure for me.