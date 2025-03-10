/*pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/AnthonyDiazCabanillas/hospital.git'
        BRANCH = 'main'
        DEPLOY_DIR = 'D:/hospital-build' // Carpeta en el disco D:
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                echo 'Clonando el repositorio...'
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }

        stage('Instalar dependencias') {
            steps {
                echo 'Instalando dependencias...'
                sh 'npm install' // Instala dependencias de Node.js
            }
        }

        stage('Compilar proyecto') {
            steps {
                echo 'Compilando el proyecto...'
                sh 'npm run build' // Compila el proyecto
            }
        }

        stage('Publicar en disco local') {
            steps {
                echo 'Publicando los archivos en el disco D:...'
                bat '''
                    if not exist "${DEPLOY_DIR}" (
                        mkdir "${DEPLOY_DIR}"
                    )
                    xcopy /E /I /Y "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\dist\\*" "${DEPLOY_DIR}"
                '''
            }
        }
    }

    post {
        success {
            echo 'El proyecto se ha compilado y publicado correctamente en el disco D:.'
        }
        failure {
            echo 'Hubo un error en la compilación o publicación del proyecto.'
        }
    }
}*/


/*pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/AnthonyDiazCabanillas/hospital.git'
        BRANCH = 'main' // Cambia a la rama que desees compilar
        DEPLOY_DIR = 'D:/WebHCE' // Carpeta de despliegue en el disco D:
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                echo 'Clonando el repositorio...'
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }

        stage('Instalar dependencias') {
            steps {
                echo 'Instalando dependencias...'
                sh 'npm install' // Instala dependencias de Node.js
            }
        }

        stage('Compilar proyecto') {
            steps {
                echo 'Compilando el proyecto...'
                sh 'npm run build' // Compila el proyecto
            }
        }

        stage('Publicar en carpeta WebHCE') {
            steps {
                echo 'Publicando los archivos en la carpeta WebHCE...'
                bat '''
                    if not exist "${DEPLOY_DIR}" (
                        mkdir "${DEPLOY_DIR}"
                    )
                    xcopy /E /I /Y "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\dist\\*" "${DEPLOY_DIR}"
                '''
            }
        }
    }

    post {
        success {
            echo 'El proyecto se ha compilado y publicado correctamente en la carpeta WebHCE.'
        }
        failure {
            echo 'Hubo un error en la compilación o publicación del proyecto.'
        }
    }
}
*/

pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/AnthonyDiazCabanillas/hospital.git'
        BRANCH = 'main' // Cambia a la rama que desees compilar
        SOLUTION_FILE = 'WebHCE.sln' // Nombre del archivo de solución
        DEPLOY_DIR = 'D:/WebHCE-Deploy' // Carpeta de despliegue en el disco D:
        MSBUILD_PATH = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\MSBuild.exe' // Ruta de MSBuild
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                echo 'Clonando el repositorio...'
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }

        stage('Restaurar paquetes NuGet') {
            steps {
                echo 'Restaurando paquetes NuGet...'
                bat 'nuget restore "${WORKSPACE}\\${SOLUTION_FILE}"'
            }
        }

        stage('Compilar solución') {
            steps {
                echo 'Compilando la solución...'
                bat "\"${MSBUILD_PATH}\" \"${WORKSPACE}\\${SOLUTION_FILE}\" /p:Configuration=Release /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=FolderProfile"
            }
        }

        stage('Publicar en carpeta de despliegue') {
            steps {
                echo 'Publicando los archivos en la carpeta de despliegue...'
                bat '''
                    if not exist "${DEPLOY_DIR}" (
                        mkdir "${DEPLOY_DIR}"
                    )
                    xcopy /E /I /Y "${WORKSPACE}\\WebHCE\\bin\\Release\\Publish\\*" "${DEPLOY_DIR}"
                '''
            }
        }
    }

    post {
        success {
            echo 'La solución se ha compilado y publicado correctamente en la carpeta de despliegue.'
        }
        failure {
            echo 'Hubo un error en la compilación o publicación de la solución.'
        }
    }
}