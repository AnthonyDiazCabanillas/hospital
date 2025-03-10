/*pipeline {
    agent any

    environment {
        // Configura las rutas y variables necesarias
        SOLUTION_FILE = "WebHCE.sln" // Nombre del archivo de solución
        PROJECT_FILE = "WebHCE.vbprj" // Nombre del archivo de proyecto
        CONFIGURATION = "Release" // Configuración de compilación (Release/Debug)
        PUBLISH_DIR = "publish" // Carpeta donde se publicarán los archivos compilados
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                // Clona el repositorio de GitHub
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Compilar proyecto') {
            steps {
                // Compila el proyecto utilizando MSBuild
                bat "msbuild ${SOLUTION_FILE} /p:Configuration=${CONFIGURATION} /p:Platform=\"Any CPU\" /t:Build"
            }
        }

        stage('Publicar proyecto') {
            steps {
                // Publica el proyecto en una carpeta específica
                bat 'msbuild C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\WebHCE\\WebHCE.vbproj /p:Configuration=Release /p:Platform="Any CPU" /p:DeployOnBuild=true /p:PublishProfile=FolderProfile'
            }
        }

        stage('Desplegar') {
            steps {
                // Copia los archivos publicados a un servidor o directorio de destino
                bat "xcopy /Y /E /I ${PUBLISH_DIR} \"D:\\DigitalizacionHC\\PruebaHospital"
            }
        }
    }

    post {
        success {
            // Acciones a realizar si el pipeline tiene éxito
            echo 'Pipeline completado con éxito.'
        }
        failure {
            // Acciones a realizar si el pipeline falla
            echo 'Pipeline fallido.'
        }
    }
}*/
pipeline {
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
                bat "xcopy /E /I /Y \"${WORKSPACE}\\dist\\*\" \"${DEPLOY_DIR}\"" // Copia los archivos al disco D:
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
}