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
        JAVA_HOME = 'C:\\Program Files\\Java\\jdk-23' // Ajusta la ruta según tu entorno en Windows
        PATH = "${JAVA_HOME}\\bin;${PATH}"
    }

    tools {
        maven 'Maven 3.9.9' // Usa el nombre que configuraste en Jenkins
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Compilar') {
            steps {
                sh 'mvn clean compile'
            }
        }

        stage('Test') {
            steps {
                sh 'mvn test'
            }
        }

        stage('Empaquetar') {
            steps {
                sh 'mvn package'
            }
        }
    }

    post {
        success {
            echo '¡Construcción exitosa!'
        }
        failure {
            echo '¡Construcción fallida!'
        }
    }
}