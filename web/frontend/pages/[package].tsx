import { Box, CircularProgress, Divider, Flex, Heading, useToast } from '@chakra-ui/react'
import { GetServerSideProps, NextPage } from 'next'
import { useEffect, useState } from 'react'

interface Props {
  packageName: string
}

interface UserResponse {
  id: string
  username: string
}

interface PackageVersionResponse {
  version: string
  timePublished: number
}

interface PackageResponse {
  name: string
  author: UserResponse
  description: string
  lastUpdated: number
  documentationUrl?: string
  repositoryUrl?: string
  downloads: number
  latestVersion?: string
  isOwnedByUser: boolean
  versions: PackageVersionResponse[] // sorted: most recent is index 0
}

const PackagePage: NextPage<Props> = ({ packageName }) => {
  const [pkg, setPkg] = useState<PackageResponse | null>(null)

  const toast = useToast()

  const fetchPackageDetails = async () => {
    const response = await fetch(`/api/v1/packages/name/${packageName}`)
    if (response.ok) {
      setPkg(await response.json())
    } else {
      toast({
        title: 'Error',
        description: `Error fetching package data: ${response.status}`,
        status: 'error',
        duration: 4000,
        isClosable: true
      })
    }
  }

  useEffect(() => {
    fetchPackageDetails()
  }, [])

  if (pkg === null) {
    return (
      <Flex justifyContent='center' alignItems='center'>
        <CircularProgress />
      </Flex>
    )
  }

  return (
    <Box p='3.5vh 10vw'>
      <Heading as='h1'>
        {packageName} {pkg.latestVersion !== null ? pkg.latestVersion : '(no versions available)'}
      </Heading>
      <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' m='25px 0' />
    </Box>
  )
}

export default PackagePage

export const getServerSideProps: GetServerSideProps<Props> = async ({ query }) => {
  return {
    props: {
      packageName: typeof query.package === 'string' ? query.package : query.package[0]
    }
  }
}
