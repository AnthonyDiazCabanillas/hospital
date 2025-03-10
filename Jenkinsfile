/*pipeline {
    agent any

    environment {
        // Configura las variables de entorno necesarias
        JAVA_HOME = '/usr/lib/jvm/java-11-openjdk-amd64' // Ajusta la ruta según tu entorno
        MAVEN_HOME = '/usr/share/maven' // Ajusta la ruta según tu entorno
        PATH = "${MAVEN_HOME}/bin:${JAVA_HOME}/bin:${PATH}"
    }

    stages {
        stage('Checkout') {
            steps {
                // Clona el repositorio
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Compilar') {
            steps {
                // Compila el proyecto usando Maven
                sh 'mvn clean compile'
            }
        }

        stage('Test') {
            steps {
                // Ejecuta las pruebas unitarias
                sh 'mvn test'
            }
        }

        stage('Empaquetar') {
            steps {
                // Empaqueta el proyecto (genera el JAR/WAR)
                sh 'mvn package'
            }
        }
    }

    post {
        success {
            // Acciones a realizar si la construcción es exitosa
            echo '¡Construcción exitosa!'
        }
        failure {
            // Acciones a realizar si la construcción falla
            echo '¡Construcción fallida!'
        }
    }
}

*/
pipeline {
    agent any

    environment {
        // Configura las rutas y variables necesarias
        SOLUTION_FILE = "WebHCE.sln" // Nombre del archivo de solución
        PROJECT_FILE = "WebHCE.vbproj" // Nombre del archivo de proyecto
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

        stage('Build') {
            steps {
                bat 'msbuild C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital\\WebHCE\\WebHCE.vbproj /p:Configuration=Release /p:Platform="Any CPU" /p:DeployOnBuild=true /p:PublishProfile=FolderProfile'
                echo 'Build completed.'
            }
        }
        stage('Publicar proyecto') {
            steps {
                // Publica el proyecto en una carpeta específica
                bat "msbuild ${PROJECT_FILE} /p:Configuration=${CONFIGURATION} /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=FolderProfile"
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
}