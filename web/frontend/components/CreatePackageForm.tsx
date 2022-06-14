import {
  Button,
  Code,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  InputGroup,
  InputLeftAddon,
  Text,
  useToast
} from '@chakra-ui/react'
import { useRouter } from 'next/router'
import { FC, FormEvent, useState } from 'react'
import StyledButton from './StyledButton'
import StyledInput from './StyledInput'

interface Props {
  onSubmit: (name: string) => void
}

const HTTPS_LENGTH = 'https://'.length
const PACKAGE_NAME_REGEX = new RegExp(/[a-z|0-9]+(-[a-z|0-9]+)*/)

const CreatePackageForm: FC<Props> = ({ onSubmit }) => {
  const [packageName, setPackageName] = useState('')
  const [packageNameError, setPackageNameError] = useState<string | null>(null)
  const [packageDescription, setPackageDescription] = useState('')
  const [packageDescriptionError, setPackageDescriptionError] = useState<string | null>(null)
  const [repositoryUrl, setRepositoryUrl] = useState('')
  const [repositoryError, setRepositoryError] = useState<string | null>(null)
  const [documentationUrl, setDocumentationUrl] = useState('')
  const [documentationError, setDocumentationError] = useState<string | null>(null)

  const router = useRouter()
  const toast = useToast()

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault()
    let isError = false
    setPackageNameError(null)
    setPackageDescriptionError(null)
    setRepositoryError(null)
    setDocumentationError(null)
    if (packageName.length > 32) {
      setPackageNameError('Package name needs to be at most 32 characters.')
      isError = true
    } else if (packageName.length < 3) {
      setPackageNameError('Package name needs to be at least 3 characters.')
      isError = true
    } else if (!PACKAGE_NAME_REGEX.test(packageName)) {
      setPackageNameError(
        'Package name must only contain letters and numbers separated by dashes (of the form /[a-z|0-9]+(-[a-z|0-9]+)*/)'
      )
    }
    if (packageDescription.length > 200) {
      setPackageDescriptionError('Package description needs to be at most 200 characters.')
      isError = true
    } else if (packageDescription.length === 0) {
      setPackageDescriptionError('Package description cannot be empty.')
      isError = true
    }
    if (repositoryUrl.length + HTTPS_LENGTH > 5000) {
      setRepositoryError('Repository URL needs to be at most 5000 characters.')
      isError = true
    }
    if (documentationUrl.length + HTTPS_LENGTH > 5000) {
      setDocumentationError('Repository URL needs to be at most 5000 characters.')
      isError = true
    }
    if (isError) {
      return
    }
    let response: Response
    try {
      response = await fetch('/api/v1/packages', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: packageName,
          description: packageDescription,
          documentationUrl: documentationUrl ? 'https://' + documentationUrl : undefined,
          repositoryUrl: repositoryUrl ? 'https://' + repositoryUrl : undefined
        })
      })
      if (response.ok) {
        onSubmit(packageName)
      } else if (response.status === 401 || response.status === 403) {
        router.push('/api/v1/oauth2/code/github')
      } else {
        toast({
          title: 'Error',
          description: `Error creating package: ${response.statusText} (${response.status})`,
          status: 'error',
          duration: 9000,
          isClosable: true
        })
      }
    } catch {
      router.push('/api/v1/oauth2/code/github')
    }
  }

  const handleRepositoryBlur = () => {
    if (repositoryUrl.startsWith('https://')) {
      setRepositoryUrl(repositoryUrl.substring(HTTPS_LENGTH))
    } else if (repositoryUrl.startsWith('http://')) {
      setRepositoryUrl(repositoryUrl.substring(HTTPS_LENGTH - 1))
    }
  }
  const handleDocumentationBlur = () => {
    if (documentationUrl.startsWith('https://')) {
      setDocumentationUrl(documentationUrl.substring(HTTPS_LENGTH))
    } else if (documentationUrl.startsWith('http://')) {
      setDocumentationUrl(documentationUrl.substring(HTTPS_LENGTH - 1))
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <FormControl isRequired>
        <FormLabel htmlFor='name'>Package name</FormLabel>
        <StyledInput id='name' value={packageName} onChange={e => setPackageName(e.target.value)} />
        {!packageNameError ? (
          <FormHelperText>
            Enter a unique name that describes your package. This should be in the form <Code>package-name</Code> (i.e., all
            lowercase and only using alphanumeric characters and dashes).
          </FormHelperText>
        ) : (
          <FormErrorMessage>{packageNameError}</FormErrorMessage>
        )}
      </FormControl>
      <FormControl isRequired>
        <FormLabel htmlFor='description' mt='15px'>
          Package description
        </FormLabel>
        <StyledInput id='description' value={packageDescription} onChange={e => setPackageDescription(e.target.value)} />
        {!packageNameError ? (
          <FormHelperText>Enter a brief (&lt;200 characters) description of your package.</FormHelperText>
        ) : (
          <FormErrorMessage>{packageDescriptionError}</FormErrorMessage>
        )}
        <Text color='gray' fontSize='11px'>
          {packageDescription.length} character{packageDescription.length !== 1 ? 's' : ''}
        </Text>
      </FormControl>

      <FormControl>
        <FormLabel htmlFor='repository' mt='15px'>
          Repository URL
        </FormLabel>
        <InputGroup>
          <InputLeftAddon children='https://' />
          <StyledInput
            id='repository'
            value={repositoryUrl}
            onChange={e => setRepositoryUrl(e.target.value)}
            onBlur={handleRepositoryBlur}
          />
        </InputGroup>
        {!repositoryError ? (
          <FormHelperText>Enter a URL for a repository for your package (optional).</FormHelperText>
        ) : (
          <FormErrorMessage>{repositoryError}</FormErrorMessage>
        )}
      </FormControl>

      <FormControl>
        <FormLabel htmlFor='documentation' mt='15px'>
          Documentation URL
        </FormLabel>
        <InputGroup>
          <InputLeftAddon children='https://' />
          <StyledInput
            id='documentation'
            value={documentationUrl}
            onChange={e => setDocumentationUrl(e.target.value)}
            onBlur={handleDocumentationBlur}
          />
        </InputGroup>
        {!documentationError ? (
          <FormHelperText>Enter a URL directing to the documentation page for your package (recommended).</FormHelperText>
        ) : (
          <FormErrorMessage>{documentationError}</FormErrorMessage>
        )}
      </FormControl>
      <StyledButton type='submit' colorScheme='teal' bgColor='tealHero' mt='10px'>
        Create
      </StyledButton>
    </form>
  )
}

export default CreatePackageForm
